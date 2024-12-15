using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence;

public class BankingDbInitializer
{
    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await roleManager.RoleExistsAsync("Client"))
        {
            await roleManager.CreateAsync(new IdentityRole("Client"));
        }
    }
}