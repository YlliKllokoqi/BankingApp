using BankingApp.Application.DTOs;

namespace BankingApp.Application.Services.Auth;

public interface IAuthService
{
    Task<ResultDto<string>> LoginAsync(LoginDto loginDto);
    Task<ResultDto<ICollection<UserDto>>> GetUsers();
    Task<ResultDto<UserDto>> FindByUserIdASync(string id);
    Task<ResultDto<bool>> RegisterUserAsync(RegisterDto registerDto);
    Task<ResultDto<bool>> DeleteUserAsync(string userId);
    Task<ResultDto<bool>> UpdateUserAsync(string userId, UpdateDto updateDto);
}