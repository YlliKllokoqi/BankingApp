using BankingApp.Domain.Entities;

namespace Infrastructure.Persistence.Repositories.DebitCard;

public interface IDebitCardRepository
{
    Task<Result<bool>> RequestDebitCard(string userId);
    Task<(Result<string>, string)> ApproveDebitCard(Guid debitCardId);
    Task<Result<List<BankingApp.Domain.Entities.DebitCard>>> GetAllDebitCards();
    Task<Result<BankingApp.Domain.Entities.DebitCard>> GetDebitCardDetails(string userId);
    Task<Result<string>> DepositToBalance(Guid debitCardId, decimal amount);
    Task<Result<decimal>> WithdrawFromBalance(Guid debitCardId, decimal amount);

}