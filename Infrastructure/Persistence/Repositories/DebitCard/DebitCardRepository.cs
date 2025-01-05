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
    
    public async Task<Result<bool>> RequestDebitCard(string UserId)
    {
        var hasDebitCard = _context.DebitCards.AnyAsync(x => x.OwnerId == UserId);
        if (hasDebitCard.Result)
        {
            return Result<bool>.Failure(new ErrorResponse
            {
                Message = "DEBIT_CARD_CREATION_FAILED",
                Details = "User already has a debit card"
            });
        }

        var firstName = _context.Users.FirstOrDefaultAsync(x => x.Id == UserId).Result.FirstName;
        var lastName = _context.Users.FirstOrDefaultAsync(x => x.Id == UserId).Result.LastName;
        
        var ownerName = firstName + " " + lastName; 
        var cardNumber = GenerateCardNumber();
        var cvv = GenerateCvv();
        var iban = GenerateIban();

        var newCard = new BankingApp.Domain.Entities.DebitCard
        {
            Id = Guid.NewGuid(),
            OwnerId = UserId,
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