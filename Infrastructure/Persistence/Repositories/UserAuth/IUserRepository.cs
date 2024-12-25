using BankingApp.Domain.Entities;

namespace Infrastructure.Persistence.Repositories.UserAuth;

public interface IUserRepository
{
    public Task<Result<ApplicationUser>> LoginAsync(ApplicationUser loginDto);
    public Task<Result<ICollection<ApplicationUser>>> GetAllUsersAsync();
    public Task<Result<ApplicationUser>> FindByUserIdAsync(string userId);
    public Task<Result<bool>> AddUserAsync(ApplicationUser user, string password);
    public Task<Result<bool>> DeleteUserAsync(string userId);
    
    public Task<Result<bool>> UpdateUserAsync(string UserId, ApplicationUser user);
}