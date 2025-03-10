using AutoMapper;
using BankingApp.Application.DTOs;
using BankingApp.Domain.Entities;
using Infrastructure.Persistence.Repositories.Transactions;

namespace BankingApp.Application.Services.Transactions;

public class TransactionsService : ITransactionService
{
    private readonly IMapper _mapper;
    private readonly ITransactionRepository _repository;

    public TransactionsService(IMapper mapper, ITransactionRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }
    
    public async Task<ResultDto<bool>> TransferFunds(TransactionDto transactionDto, string userId)
    {
        var transaction = _mapper.Map<Transaction>(transactionDto);
        var result = await _repository.TransferFunds(transaction);

        return _mapper.Map<ResultDto<bool>>(result);
    }
}