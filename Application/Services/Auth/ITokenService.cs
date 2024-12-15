using BankingApp.Domain.Entities;

namespace BankingApp.Application.Services.Auth;

public interface ITokenService
{
    public string GenerateJwtToken(ApplicationUser user);
}