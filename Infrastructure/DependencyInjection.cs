using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using BankingApp.Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories.UserAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<BankingDbContext>(options => options.UseNpgsql(configuration["ConnectionStrings:DefaultConnection"]));
		services.AddIdentityCore<ApplicationUser>(options =>
			{
				options.User.RequireUniqueEmail = true;
				options.Tokens.AuthenticatorTokenProvider = null;
			})
			.AddRoles<IdentityRole>() // Add role support
			.AddEntityFrameworkStores<BankingDbContext>()
			.AddRoleManager<RoleManager<IdentityRole>>() // RoleManager for role handling
			.AddUserManager<UserManager<ApplicationUser>>() // UserManager for user handling
			.AddSignInManager<SignInManager<ApplicationUser>>();
		
		services.AddSingleton<TimeProvider>(TimeProvider.System);
		
		services.AddTransient<BankingDbInitializer>();
		
		services.AddScoped<IUserRepository, UserRepository>();

		return services;
	}
}
