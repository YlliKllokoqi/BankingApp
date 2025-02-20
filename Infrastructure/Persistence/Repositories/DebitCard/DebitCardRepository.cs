using BankingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.DebitCard;

public class DebitCardRepository : IDebitCardRepository
{
    private readonly BankingDbContext _context;
    private static Random _random = new Random();

    public DebitCardRepository(BankingDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<bool>> RequestDebitCard(string userId)
    {
        var hasDebitCard = _context.DebitCards.AnyAsync(x => x.OwnerId == userId);
        if (hasDebitCard.Result)
        {
            return Result<bool>.Failure(new ErrorResponse
            {
                Message = "DEBIT_CARD_CREATION_FAILED",
                Details = "User already has a debit card"
            });
        }

        var firstName = _context.Users.FirstOrDefaultAsync(x => x.Id == userId).Result.FirstName;
        var lastName = _context.Users.FirstOrDefaultAsync(x => x.Id == userId).Result.LastName;
        
        var ownerName = firstName + " " + lastName; 
        var cardNumber = GenerateCardNumber();
        var cvv = GenerateCvv();
        var iban = GenerateIban();

        var newCard = new BankingApp.Domain.Entities.DebitCard
        {
            Id = Guid.NewGuid(),
            OwnerId = userId,
            OwnerName = ownerName,
            CardNumber = cardNumber.Result,
            CVV = cvv.Result,
            IBAN = iban.Result,
            Status = "Pending",
            ExpirationDate = DateTime.UtcNow.AddYears(5),
        };

        await _context.DebitCards.AddAsync(newCard);
        return await _context.SaveChangesAsync() > 0  
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(new ErrorResponse
            {
                Message = "DEBIT_CARD_FAILED",
                Details = "Debit card creation failed"
            }); 
    }

    public async Task<(Result<string>, string)> ApproveDebitCard(Guid debitCardId)
    {
        var debitCard = _context.DebitCards.FirstOrDefaultAsync(x => x.Id == debitCardId).Result;
        
        if(debitCard == null)
            return (Result<string>.Failure(new ErrorResponse
            {
                Message = "DEBIT_CARD_APPROVAL_FAILED",
                Details = "debit card does not exist"
            }), String.Empty);

        if (debitCard.Status == "Active")
        {
            return (Result<string>.Failure(new ErrorResponse
            {
                Message = "DEBIT_CARD_APPROVAL_FAILED",
                Details = "Debit card is already active",
            }), String.Empty);
        }

        debitCard.Status = "Active";
        _context.DebitCards.Update(debitCard);

        if (await _context.SaveChangesAsync() > 0)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == debitCard.OwnerId);
            
            return (Result<string>.Success("Debbit card has been approved"), user.Email);
        }
        else
        {
            return (Result<string>.Failure(new ErrorResponse
            {
                Message = "DEBIT_CARD_APPROVAL_FAILED",
                Details = "DB context changes could not be saved",
            }), String.Empty);
        }

    }

    public async Task<Result<List<BankingApp.Domain.Entities.DebitCard>>> GetAllDebitCards()
    {
        var result = await _context.DebitCards.ToListAsync();
        
        return Result<List<BankingApp.Domain.Entities.DebitCard>>.Success(result);
    }

    public async Task<Result<BankingApp.Domain.Entities.DebitCard>> GetDebitCardDetails(string userId)
    {
        var result = await _context.DebitCards.FirstOrDefaultAsync(x => x.OwnerId == userId);

        if (result != null)
        {
            return Result<BankingApp.Domain.Entities.DebitCard>.Success(result);
        }
        else
        {
            return Result<BankingApp.Domain.Entities.DebitCard>.Failure(new ErrorResponse
            {
                Message = "DEBIT_CARD_RETREIVAL_FAILED",
                Details = "Debit card not found"
            });
        }
    }

    public async Task<Result<string>> DepositToBalance(Guid debitCardId, decimal amount)
    {
        for (int retry = 0; retry < 3; retry++)
        {
            try
            {
                var debitCard = await _context.DebitCards.FirstOrDefaultAsync(d => d.Id == debitCardId);

                if (debitCard == null)
                    return Result<string>.Failure(new ErrorResponse
                    {
                        Message = "DEBIT_CARD_NOT_FOUND",
                        Details = "Debit card does not exist"
                    });

                debitCard.Balance += amount;
                await _context.SaveChangesAsync();

                return Result<string>.Success(amount + " has been deposited to your balance");
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (retry == 2)
                    return Result<string>.Failure(new ErrorResponse
                    {
                        Message = "DEPOSIT_FAILED",
                        Details = "Unable to deposit to your balance, please try again: " + e.Message
                    });
            }
        }
        return Result<string>.Failure(new ErrorResponse
        {
            Message = "DEPOSIT_FAILED",
            Details = "Unable to deposit to your balance, please try again"
        });
    }

    public async Task<Result<decimal>> WithdrawFromBalance(Guid debitCardId, decimal amount)
    {
        for (int retry = 0; retry < 3; retry++)
        {
            var debitCard = await _context.DebitCards.FirstOrDefaultAsync(d => d.Id == debitCardId);
            try
            {
                if(debitCard == null)
                    return Result<decimal>.Failure(new ErrorResponse
                    {
                        Message = "DEBIT_CARD_NOT_FOUND",
                        Details = "Debit card does not exist"
                    });
                
                if(debitCard.Balance == 0)
                    return Result<decimal>.Failure(new ErrorResponse
                    {
                        Message = "WITHDRAWAL_FAILED",
                        Details = "You do not have sufficient funds"
                    });
                
                debitCard.Balance = Math.Max(0, debitCard.Balance - amount);
                
                await _context.SaveChangesAsync();

                return Result<decimal>.Success(debitCard.Balance);
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (retry == 2)
                    return Result<decimal>.Failure(new ErrorResponse
                    {
                        Message = "WITHDRAWAL_FAILED",
                        Details = "Unable to withdraw from your balance, please try again"
                    });
            }
        }
        return Result<decimal>.Failure(new ErrorResponse
        {
            Message = "WITHDRAWAL_FAILED",
            Details = "Unable to Withdraw from your balance, please try again"
        });
    }

    private async Task<string> GenerateIban()
    {
        while (true)
        {
            string cvv = string.Concat(Enumerable.Range(0, 16).Select(_ => _random.Next(0, 10).ToString()));

            if (!await _context.DebitCards.AnyAsync(x => x.CVV == cvv))
                return cvv;
        }
    }


    private async Task<string> GenerateCvv()
    {
        while (true)
        {
            string cvv = string.Concat(Enumerable.Range(0, 3).Select(_ => _random.Next(0, 10).ToString()));

            if (!await _context.DebitCards.AnyAsync(x => x.CVV == cvv))
                return cvv;
        }
    }


    private async Task<string> GenerateCardNumber()
    {
        while (true)
        {
            string cardNumber = string.Concat(Enumerable.Range(0, 16).Select(_ => _random.Next(0, 10).ToString()));
            
            if (!await _context.DebitCards.AnyAsync(x => x.CardNumber == cardNumber))
                return cardNumber;
        }
    }
}