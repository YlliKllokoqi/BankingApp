using BankingApp.Domain.Entities;

namespace Infrastructure.Persistence.Repositories.DebitCard;

public interface IDebitCardRepository
{
    Task<Result<bool>> RequestDebitCard(string UserId);
}