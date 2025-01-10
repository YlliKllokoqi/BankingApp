using BankingApp.Application.DTOs;

namespace BankingApp.Application.Services.Email;

public interface IEmailService
{
    Task<ResultDto<string>> SendDebitCardApprovalEmail(string email);
}