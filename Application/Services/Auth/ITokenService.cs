using BankingApp.Domain.Entities;

namespace BankingApp.Application.Services.Auth;

public interface ITokenService
{
    public Task<string> GenerateJwtToken(ApplicationUser user);
}