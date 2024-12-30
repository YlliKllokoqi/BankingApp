using BankingApp.Application.DTOs;
using BankingApp.Domain.Entities;

namespace BankingApp.Application.Services.Auth;

public interface IAuthService
{
    Task<ResultDto<string>> LoginAsync(LoginDto loginDto);
    Task<ResultDto<ICollection<UserDto>>> GetUsers();
    Task<ResultDto<UserDto>> FindUserById(string id);
    Task<ResultDto<bool>> RegisterUserAsync(RegisterDto registerDto);
    Task<ResultDto<bool>> DeleteUserAsync(string userId);
    Task<ResultDto<bool>> UpdateUserAsync(string userId, UpdateDto updateDto);
    Task<ResultDto<bool>> AssignRole(string userId, string role);
}