using BankingApp.Domain.Entities;

namespace Infrastructure.Persistence.Repositories.Transactions;

public interface ITransactionRepository
{
    Task<Result<bool>> TransferFunds(Transaction transaction);
    Task<(List<Transaction>, int)> GetTransactionHistory(TransactionQuery queryDto);
}