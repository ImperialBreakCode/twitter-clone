using Microsoft.AspNetCore.Identity;
using TwitterUni.Constants;
using TwitterUni.Data.Entities;

namespace TwitterUni
{
    public static class AppInit
    {
        public static async Task<WebApplication> SeedRolesAndAdminUserAsync(this WebApplication app)
        {
            using (IServiceScope scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetService<UserManager<User>>();

                if (roleManager is not null)
                {
                    await EnsureRole(RoleNames.User, roleManager);
                    await EnsureRole(RoleNames.Moderator, roleManager);
                    await EnsureRole(RoleNames.Admin, roleManager);

                    if (userManager is not null)
                    {
                        await EnsureAdmin(userManager);
                    }
                }
            }

            return app;
        }

        private static async Task EnsureRole(string roleName, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                IdentityRole userRole = new IdentityRole(roleName);
                await roleManager.CreateAsync(userRole);
            }
        }

        private static async Task EnsureAdmin(UserManager<User> userManager)
        {
            if (!(await userManager.GetUsersInRoleAsync(RoleNames.Admin)).Any())
            {
                User user = new User()
                {
                    FirstName = "Admin",
                    LastName = "Seed",
                    UserName = "admin",
                    IsSet = false,
                };

                await userManager.CreateAsync(user, "Pass_word123");
                await userManager.AddToRoleAsync(user, RoleNames.Admin);
            }
        }
    }
}
