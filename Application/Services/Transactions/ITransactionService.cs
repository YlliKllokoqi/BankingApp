using BankingApp.Application.DTOs;

namespace BankingApp.Application.Services.Transactions;

public interface ITransactionService
{
    Task<ResultDto<bool>> TransferFunds(TransactionDto transactionDto, string userId);   
}