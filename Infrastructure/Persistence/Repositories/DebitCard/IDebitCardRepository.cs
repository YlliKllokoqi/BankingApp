using BankingApp.Domain.Entities;

namespace Infrastructure.Persistence.Repositories.DebitCard;

public interface IDebitCardRepository
{
    Task<Result<bool>> RequestDebitCard(string userId);
    Task<(Result<string>, string)> ApproveDebitCard(Guid debitCardId);

    Task<Result<BankingApp.Domain.Entities.DebitCard>> GetDebitCardDetails(string userId);
}