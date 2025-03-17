using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MotorcycleForum.Data.Entities;
using System;
using System.Threading.Tasks;

public static class RoleSeeding
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        string[] roleNames = { "User", "Moderator", "Admin" };
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                // Use the constructor with only the role name
                var role = new IdentityRole<Guid>(roleName);
                roleResult = await roleManager.CreateAsync(role);
            }
        }

        // Seed Admin User
        Guid adminId = Guid.Parse("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8");
        string adminEmail = "motosphere.site@gmail.com";
        string adminPassword = "X;W0Q6^Ej0Xc";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new User
            {
                Id = adminId,
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var createAdmin = await userManager.CreateAsync(adminUser, adminPassword);
            if (createAdmin.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
