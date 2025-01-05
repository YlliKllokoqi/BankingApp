using AutoMapper;
using BankingApp.Application.DTOs;
using FluentValidation;
using Infrastructure.Persistence.Repositories.DebitCard;

namespace BankingApp.Application.Services.DebitCard;

public class DebitCardService : IDebitCardService
{
    private readonly IDebitCardRepository _debitCardRepository;
    private readonly IMapper _mapper;

    public DebitCardService(IDebitCardRepository debitCardRepository,
                            IMapper mapper)
    {
        _debitCardRepository = debitCardRepository;
        _mapper = mapper;
    }
    
    public async Task<ResultDto<bool>> RequestDebitCard(string userId)
    {
        var result = await _debitCardRepository.RequestDebitCard(userId);

        if (!result.IsSuccess)
        {
            return ResultDto<bool>.Failure(new ErrorResponseDto
            {
                Message = "Debit Card request failed",
                Details = result.Error.Details
            });
        }

        return ResultDto<bool>.Success(true);
    }
}