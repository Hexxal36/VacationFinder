namespace VacationFinder.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using VacationFinder.Common;
    using VacationFinder.Data;
    using VacationFinder.Data.Models;
    using VacationFinder.Services.Messaging;

    public class OrderController : AdministrationController
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender emailSender;

        public OrderController(
            ApplicationDbContext context,
            IEmailSender emailSender)
        {
            this._context = context;
            this.emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            return this.View(await this._context.Orders.ToListAsync());
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var order = await this._context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return this.NotFound();
            }

            return this.View(order);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await this._context.Orders.FindAsync(id);
            order.IsDeleted = true;
            order.DeletedOn = DateTime.UtcNow;
            await this._context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Approve(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var order = await this._context.Orders.FindAsync(id);
            if (order == null)
            {
                return this.NotFound();
            }

            return this.View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            Order order = await this._context.Orders.FindAsync(id);

            if (order != null)
            {
                order.IsApproved = true;

                this._context.Update(order);
                await this._context.SaveChangesAsync();

                await this.emailSender.SendEmailAsync(
                    EmailConstants.From,
                    "Admin",
                    order.ContactEmail,
                    EmailConstants.Subject,
                    EmailConstants.Body);
            }

            return this.View(order);
        }

        private bool OrderExists(int id)
        {
            return this._context.Orders.Any(e => e.Id == id);
        }
    }
}
