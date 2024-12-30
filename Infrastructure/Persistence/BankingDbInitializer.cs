using BankingApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence;

public class BankingDbInitializer
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public BankingDbInitializer(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }
    
    public async Task SeedRoles()
    {
        if (!await _roleManager.RoleExistsAsync("Admin"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await _roleManager.RoleExistsAsync("User"))
        {
            await _roleManager.CreateAsync(new IdentityRole("User"));
        }
    }

    public async Task SeedAdminAsync()
    {
        const string adminEmail = "admin@admin.com";
        const string adminUserName = "BaseAdmin";
        const string adminPassword = "Admin123!";

        if (await _userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminUserName,
                Email = adminEmail,
                Password = adminPassword,
                FirstName = "Admin",
                LastName = "Adminos",
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                Gender = Gender.M,
                PhoneNumber = "",
                Address = "",
                PostalCode = ""
            };
            
            var result = await _userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}