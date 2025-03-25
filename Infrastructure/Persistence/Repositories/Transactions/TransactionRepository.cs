using BankingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Transactions;

public class TransactionRepository : ITransactionRepository
{
    private readonly BankingDbContext _context;

    public TransactionRepository(BankingDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<bool>> TransferFunds(Transaction transactionModel)
    {
        await using var transactionState = await _context.Database.BeginTransactionAsync();

        try
        {
            var sourceDebitCard = await _context.DebitCards.FirstOrDefaultAsync(d => d.Id == transactionModel.SourceDebitCardId);
            var destinationDebitCard =
                await _context.DebitCards.FirstOrDefaultAsync(d => d.IBAN == transactionModel.IBAN);

            if (sourceDebitCard == null || destinationDebitCard == null)
            {
                return Result<bool>.Failure(new ErrorResponse
                {
                    Message = "TRANSACTION_FAILED",
                    Details = sourceDebitCard is null ? "Source account" : "Destination account" + " does not exist"
                });
            }

            if (sourceDebitCard.Balance < transactionModel.Amount)
            {
                return Result<bool>.Failure(new ErrorResponse
                {
                    Message = "TRANSACTION_FAILED",
                    Details = "Inssufficient funds"
                });
            }

            sourceDebitCard.Balance -= transactionModel.Amount;
            destinationDebitCard.Balance += transactionModel.Amount;

            await _context.SaveChangesAsync();

            var recipient = await _context.DebitCards
                .Where(d => d.IBAN == transactionModel.IBAN).Select(d => d.OwnerName).FirstOrDefaultAsync();

            var transaction = new Transaction
            {
                Id = new Guid(),
                Amount = transactionModel.Amount,
                Description = transactionModel.Description,
                SourceDebitCardId = transactionModel.SourceDebitCardId,
                IBAN = transactionModel.IBAN,
                Recipient = recipient,
                Sender = transactionModel.Sender,
                Date = DateTime.UtcNow
            };
            
            await _context.AddAsync(transaction);
            await _context.SaveChangesAsync();
            await transactionState.CommitAsync();

            return Result<bool>.Success(true);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            await transactionState.RollbackAsync();
            return Result<bool>.Failure(new ErrorResponse
            {
                Message = "TRANSACTION_FAILED",
                Details = "Concurrency exception occured, please try again later: " + ex.Message
            });
        }
        catch (Exception ex)
        {
            await transactionState.RollbackAsync();
            return Result<bool>.Failure(new ErrorResponse
            {
                Message = "TRANSACTION_FAILED",
                Details = ex.Message
            });
        }
    }

    public async Task<(List<Transaction>, int)> GetTransactionHistory(TransactionQuery query)
    {
        var userDebitCard = await _context.DebitCards
            .Where(d => d.OwnerId == query.UserId)
            .Select(d => new { d.Id, d.IBAN })
            .FirstOrDefaultAsync();

        if (userDebitCard == null)
        {
            return new(new List<Transaction>(), 0);
        }
        
        query.StartDate = DateTime.SpecifyKind(query.StartDate, DateTimeKind.Utc);
        query.EndDate = DateTime.SpecifyKind(query.EndDate, DateTimeKind.Utc);


        var transactionQuery = _context.Transactions
            .Where(t =>
                (t.SourceDebitCardId == userDebitCard.Id || t.IBAN == userDebitCard.IBAN)
                && t.Date >= query.StartDate.ToUniversalTime() && t.Date <= query.EndDate.ToUniversalTime())
            .OrderByDescending(t => t.Date);

        int totalRecords = await transactionQuery.CountAsync();

        var transactions = await transactionQuery
            .Skip((query.pageNumber - 1) * query.pageSize)
            .Take(query.pageSize)
            .ToListAsync();

        return (transactions, totalRecords);
    }
}