using BankingApp.Application.DTOs;

namespace BankingApp.Application.Services.Auth;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDto loginDto);
    Task<ICollection<UserDto>> GetUsers();
    Task<UserDto> FindByUserIdASync(string id);
    Task<bool> RegisterUserAsync(RegisterDto registerDto);
    Task<bool> DeleteUserAsync(string userId);
    Task<bool> UpdateUserAsync(string userId, UpdateDto updateDto);
}