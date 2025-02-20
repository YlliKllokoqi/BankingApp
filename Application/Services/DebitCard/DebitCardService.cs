using AutoMapper;
using BankingApp.Application.DTOs;
using BankingApp.Application.Services.Email;
using FluentValidation;
using Infrastructure.Persistence.Repositories.DebitCard;

namespace BankingApp.Application.Services.DebitCard;

public class DebitCardService : IDebitCardService
{
    private readonly IEmailService _emailService;
    private readonly IDebitCardRepository _debitCardRepository;
    private readonly IMapper _mapper;

    public DebitCardService(IEmailService emailService,
                            IDebitCardRepository debitCardRepository,
                            IMapper mapper)
    {
        _emailService = emailService;
        _debitCardRepository = debitCardRepository;
        _mapper = mapper;
    }
    
    public async Task<ResultDto<bool>> RequestDebitCard(string userId)
    {
        var result = await _debitCardRepository.RequestDebitCard(userId);
        
        return _mapper.Map<ResultDto<bool>>(result);
    }

    public async Task<ResultDto<string>> ApproveDebitCard(Guid debitCardId)
    {
        var result = await _debitCardRepository.ApproveDebitCard(debitCardId);

        if (result.Item1.IsSuccess)
        {
            var emailResult = await _emailService.SendDebitCardApprovalEmail(result.Item2);

            return emailResult;
        }

        return ResultDto<string>.Failure(new ErrorResponseDto
        {
            Message = result.Item1.Error.Message,
            Details = result.Item1.Error.Details
        });
    }

    public async Task<ResultDto<List<DebitCardDto>>> GetAllDebitCards()
    {
        var result = await _debitCardRepository.GetAllDebitCards();

        return _mapper.Map<ResultDto<List<DebitCardDto>>>(result);
    }

    public async Task<ResultDto<DebitCardDto>> GetDebitCardDetails(string userId)
    {
        var result = await _debitCardRepository.GetDebitCardDetails(userId);

        if (result.IsSuccess)
        {
            var debitcardDto = _mapper.Map<DebitCardDto>(result.Data);
            return ResultDto<DebitCardDto>.Success(debitcardDto);
        }
        else
        {
            return ResultDto<DebitCardDto>.Failure(new ErrorResponseDto
            {
                Message = result.Error.Message,
                Details = result.Error.Details
            });
        }
    }

    public async Task<ResultDto<string>> DepositToBalance(Guid debitCardId, decimal amount)
    {
        if (amount > 3000)
            return ResultDto<string>.Failure(new ErrorResponseDto
            {
                Message = "DEPOSIT_FAILED",
                Details = "Your deposit exceeded the permitted amount, please contact our administrator"
            });
        if(amount < 0)
            return ResultDto<string>.Failure(new ErrorResponseDto
            {
                Message = "DEPOSIT_FAILED",
                Details = "Negative amount"
            });
        
        var result = await _debitCardRepository.DepositToBalance(debitCardId, amount);
        
        return _mapper.Map<ResultDto<string>>(result);
    }

    public async Task<ResultDto<string>> WithdrawFromBalance(Guid debitCardId, decimal amount)
    {
        if(amount < 10)
            return ResultDto<string>.Failure(new ErrorResponseDto
            {
                Message = "WITHDRAWAL_FAILED",
                Details = "Please withdraw at least the minimum amount of 10€"
            });
        
        var result = await _debitCardRepository.WithdrawFromBalance(debitCardId, amount);

        return _mapper.Map<ResultDto<string>>(result);
    }
}