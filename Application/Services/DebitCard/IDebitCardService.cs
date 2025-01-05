using BankingApp.Application.DTOs;

namespace BankingApp.Application.Services.DebitCard;

public interface IDebitCardService
{
    Task<ResultDto<bool>> RequestDebitCard(string userId);

}