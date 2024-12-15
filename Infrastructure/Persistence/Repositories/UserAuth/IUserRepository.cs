using BankingApp.Domain.Entities;

namespace Infrastructure.Persistence.Repositories.UserAuth;

public interface IUserRepository
{
    public Task<ApplicationUser> LoginAsync(ApplicationUser loginDto);
    public Task<ICollection<ApplicationUser>> GetAllUsersAsync();
    public Task<ApplicationUser> FindByUserIdAsync(string userId);
    public Task<bool> AddUserAsync(ApplicationUser user, string password);
    public Task<bool> DeleteUserAsync(string userId);
    
    public Task<bool> UpdateUserAsync(string UserId, ApplicationUser user);
}