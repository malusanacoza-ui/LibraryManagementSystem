using Microsoft.AspNetCore.Identity;

namespace LibraryManagementSystem.Data   // ⭐ IMPORTANT
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string adminRole = "Admin";
            string adminEmail = "lucyadmin@gmail.com";
            string adminPassword = "Malusana2@";

            // Create Role
            if (!await roleManager.RoleExistsAsync(adminRole))
                await roleManager.CreateAsync(new IdentityRole(adminRole));

            // Create Admin User
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(adminUser, adminRole);
            }
        }
    }
}
