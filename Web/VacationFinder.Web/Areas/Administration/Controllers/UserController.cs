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
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await this._context.Users.ToListAsync();

            var viewModel = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roleName = "User";

                if (await this._userManager.IsInRoleAsync(user, GlobalConstants.SuperAdministratorRoleName))
                {
                    roleName = "SuperAdmin";
                }
                else if (await this._userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName))
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
            var user = await this._context.Users.FindAsync(id);

            if (await this._userManager.IsInRoleAsync(user, GlobalConstants.SuperAdministratorRoleName))
            {
                if (await this._userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName))
                {
                    await this._userManager.RemoveFromRoleAsync(user, GlobalConstants.AdministratorRoleName);
                }
                else
                {
                    await this._userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
                }
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await this._context.Users.FindAsync(id);

            if (!await this._userManager.IsInRoleAsync(user, GlobalConstants.SuperAdministratorRoleName))
            {
                user.IsDeleted = true;
                user.DeletedOn = DateTime.Now.AddHours(-3);

                await this._context.SaveChangesAsync();
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
