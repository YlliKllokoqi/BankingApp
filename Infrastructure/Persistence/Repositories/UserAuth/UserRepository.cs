using BankingApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.UserAuth;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UserRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<ApplicationUser> LoginAsync(ApplicationUser user)
    {
        var appUser = await _userManager.FindByEmailAsync(user.Email);
        if (appUser is not null)
        {
            var result = await _signInManager
                .CheckPasswordSignInAsync(appUser, user.Password,false);

            if (result.Succeeded)
            {
                return appUser;
            }
            if (!result.Succeeded)
            {
                string errorMessage = $"Login failed for user {user.Email}. Reason: ";

                if (result.IsLockedOut)
                {
                    errorMessage += "User is locked out.";
                }
                else if (result.IsNotAllowed)
                {
                    errorMessage += "User is not allowed to log in.";
                }
                else if (result.RequiresTwoFactor)
                {
                    errorMessage += "Two-factor authentication is required.";
                }
                else
                {
                    errorMessage += "Unknown reason.";
                }

                throw new Exception(errorMessage);
            }
        }
        else
        {
            throw new Exception("User does not exist.");
        }
        
        return null;
    }

    public async Task<ICollection<ApplicationUser>> GetAllUsersAsync()
    {
        return await _userManager.Users.ToListAsync();
    }

    public async Task<ApplicationUser> FindByUserIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is not null)
        {
            return user;
        }
        else
        {
            throw new Exception("User not found");
        }
    }

    public async Task<bool> AddUserAsync(ApplicationUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        return result.Succeeded;
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> UpdateUserAsync(string userId, ApplicationUser appUser)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) throw new Exception("User not found");
        
        if(!string.IsNullOrWhiteSpace(appUser.FirstName))
            user.FirstName = appUser.FirstName;
        if(!string.IsNullOrWhiteSpace(appUser.LastName))
            user.LastName = appUser.LastName;
        if (!string.IsNullOrWhiteSpace(appUser.UserName) && appUser.UserName != user.UserName)
            user.UserName = appUser.UserName;

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }
}