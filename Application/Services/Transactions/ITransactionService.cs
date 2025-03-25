using BankingApp.Application.DTOs;
using BankingApp.Domain.Entities;

namespace BankingApp.Application.Services.Transactions;

public interface ITransactionService
{
    Task<ResultDto<bool>> TransferFunds(TransactionDto transactionDto, string userId);
    Task<PagedResult<GetTransactionDto>> GetTransactionHistory(TransactionQueryDto queryDto);
}