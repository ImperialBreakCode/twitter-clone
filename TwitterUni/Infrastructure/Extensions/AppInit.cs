using Microsoft.AspNetCore.Identity;
using TwitterUni.Data.Entities;
using TwitterUni.Infrastructure.Constants;
using TwitterUni.Services;
using TwitterUni.Services.Interfaces;

namespace TwitterUni.Infrastructure.Extensions
{
    public static class AppInit
    {
        public static async Task<WebApplication> SeedAppData(this WebApplication app)
        {
            using (IServiceScope scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
                var appSettings = scope.ServiceProvider.GetService<IAppSettingsService>();

                if (appSettings is not null)
                {
                    appSettings.EnsureAppSettings();
                }

                if (roleManager is not null)
                {
                    await EnsureRole(RoleNames.User, roleManager);
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
                    ProfilePic = "default_prf_pic.png",
                    BackgroundPhoto = "default_background.jpg"
                };

                await userManager.CreateAsync(user, "Pass_word123");
                await userManager.AddToRoleAsync(user, RoleNames.Admin);
                await userManager.AddToRoleAsync(user, RoleNames.User);
            }
        }
    }
}
