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

    public async Task<Result<ApplicationUser>> LoginAsync(ApplicationUser user)
    {
        var appUser = await _userManager.FindByEmailAsync(user.Email);
        if (appUser == null)
        {
            return Result<ApplicationUser>.Failure(new ErrorResponse
            {
                Message = "USER_NOT_FOUND",
                Details = "User with the provided email does not exist."
            });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(appUser, user.Password, false);

        if (!result.Succeeded)
        {
            return Result<ApplicationUser>.Failure(new ErrorResponse
            {
                Message = "INVALID_CREDENTIALS",
                Details = "Email/Password incorrect."
            });
        }

        return Result<ApplicationUser>.Success(appUser);
    }


    public async Task<Result<ICollection<ApplicationUser>>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        if (!users.Any())
        {
            return Result<ICollection<ApplicationUser>>.Failure(new ErrorResponse
            {
                Message = "NO_USERS_FOUND",
                Details = "There are no users in the database."
            });
        }

        return Result<ICollection<ApplicationUser>>.Success(users);
    }


    public async Task<Result<ApplicationUser>> FindUserById(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Result<ApplicationUser>.Failure(new ErrorResponse
            {
                Message = "USER_NOT_FOUND",
                Details = "No user found with the given ID."
            });
        }

        return Result<ApplicationUser>.Success(user);
    }


    public async Task<Result<bool>> AddUserAsync(ApplicationUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return Result<bool>.Failure(new ErrorResponse
            {
                Message = "User Creation Failed",
                Details = string.Join(',', result.Errors.Select(x => x.Description))
            });
        }
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Result<bool>.Failure(new ErrorResponse
            {
                Message = "USER_NOT_FOUND",
                Details = "Cannot delete a user that does not exist."
            });
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            return Result<bool>.Failure(new ErrorResponse
            {
                Message = "USER_DELETION_FAILED",
                Details = string.Join("; ", result.Errors.Select(e => e.Description))
            });
        }

        return Result<bool>.Success(true);
    }


    public async Task<Result<bool>> UpdateUserAsync(string userId, ApplicationUser appUser)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Result<bool>.Failure(new ErrorResponse
            {
                Message = "USER_NOT_FOUND",
                Details = "Cannot update a user that does not exist."
            });
        }

        if (!string.IsNullOrWhiteSpace(appUser.FirstName))
            user.FirstName = appUser.FirstName;
        if (!string.IsNullOrWhiteSpace(appUser.LastName))
            user.LastName = appUser.LastName;
        if (!string.IsNullOrWhiteSpace(appUser.UserName) && appUser.UserName != user.UserName)
            user.UserName = appUser.UserName;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return Result<bool>.Failure(new ErrorResponse
            {
                Message = "USER_UPDATE_FAILED",
                Details = string.Join("; ", result.Errors.Select(e => e.Description))
            });
        }

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> AssignRoleAsync(ApplicationUser user, string role)
    {
        var result = await _userManager.AddToRoleAsync(user, role);

        if (!result.Succeeded)
        {
            return Result<bool>.Failure(new ErrorResponse
            {
                Message = "ROLE_ASSIGNMENT_FAILED",
                Details = string.Join("; ", result.Errors.Select(e => e.Description))
            });
        }

        return Result<bool>.Success(true);
    }
}