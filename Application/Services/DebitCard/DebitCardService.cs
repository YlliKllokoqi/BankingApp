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

        if (!result.IsSuccess)
        {
            return ResultDto<bool>.Failure(new ErrorResponseDto
            {
                Message = result.Error.Message,
                Details = result.Error.Details
            });
        }

        return ResultDto<bool>.Success(true);
    }

    public async Task<ResultDto<string>> ApproveDebitCard(Guid debitCardId)
    {
        var result = await _debitCardRepository.ApproveDebitCard(debitCardId);

        if (result.Item1.IsSuccess)
        {
            var emailResult = await _emailService.SendDebitCardApprovalEmail(result.Item2);

            if (emailResult.IsSuccess)
            {
                return ResultDto<string>.Success("Debit Card approved");
            }
            else
            {
                return ResultDto<string>.Failure(new ErrorResponseDto
                {
                    Message = emailResult.Error.Message,
                    Details = emailResult.Error.Details
                });
            }
        }

        return ResultDto<string>.Failure(new ErrorResponseDto
        {
            Message = result.Item1.Error.Message,
            Details = result.Item1.Error.Details
        });
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
}