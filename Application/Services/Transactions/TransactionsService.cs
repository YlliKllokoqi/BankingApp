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

    public async Task<PagedResult<GetTransactionDto>> GetTransactionHistory(TransactionQueryDto queryDto)
    {
        var transactionquery = _mapper.Map<TransactionQuery>(queryDto);
        var (transactions, totalRecords) = await _repository.GetTransactionHistory(transactionquery);

        var transactionDtos = transactions.Select(t => new GetTransactionDto
        {
            Amount = t.Amount,
            Description = t.Description,
            Recipient = t.Recipient,
            Sender = t.Sender,
            Date = t.Date
        }).ToList();

        return new PagedResult<GetTransactionDto>
        {
            Items = transactionDtos,
            PageNumber = queryDto.pageNumber,
            PageSize = queryDto.pageSize,
            TotalRecords = totalRecords
        };
    }
}