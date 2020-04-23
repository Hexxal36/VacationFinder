namespace VacationFinder.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using VacationFinder.Common;
    using VacationFinder.Data;
    using VacationFinder.Data.Models;
    using VacationFinder.Web.ViewModels.Administration.User;

    public class UserController : AdministrationController
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await this.context.Users.ToListAsync();

            var viewModel = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roleName = "User";

                if (await this.userManager.IsInRoleAsync(user, GlobalConstants.SuperAdministratorRoleName))
                {
                    roleName = "SuperAdmin";
                }
                else if (await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName))
                {
                    roleName = "Admin";
                }

                viewModel.Add(new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    RoleName = roleName,
                });
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeAdmin(string id)
        {
            var user = await this.context.Users.FindAsync(id);

            if (await this.userManager.IsInRoleAsync(user, GlobalConstants.SuperAdministratorRoleName))
            {
                if (await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName))
                {
                    await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.AdministratorRoleName);
                }
                else
                {
                    await this.userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
                }
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await this.context.Users.FindAsync(id);

            if (!await this.userManager.IsInRoleAsync(user, GlobalConstants.SuperAdministratorRoleName))
            {
                user.IsDeleted = true;
                user.DeletedOn = DateTime.Now.AddHours(-3);

                await this.context.SaveChangesAsync();
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
