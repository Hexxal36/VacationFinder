namespace VacationFinder.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using VacationFinder.Common;
    using VacationFinder.Data.Models;

    public class AdminSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedUserAsync(userManager, "admin@mail.com", "admin@mail.com", "123456");
        }

        private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, string email, string username, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    Email = email,
                    UserName = username,
                };

                var result = await userManager.CreateAsync(user, password);

                await userManager.AddToRoleAsync(user, GlobalConstants.SuperAdministratorRoleName);
                await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
