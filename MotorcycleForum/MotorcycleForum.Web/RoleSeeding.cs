using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MotorcycleForum.Data.Entities;
using System;
using System.Threading.Tasks;

public static class SeedData
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
        string adminEmail = "motosphere.site@gmail.com";
        string adminPassword = "X;W0Q6^Ej0Xc";  // Change this in production

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new User { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            var createAdmin = await userManager.CreateAsync(adminUser, adminPassword);
            if (createAdmin.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
