namespace VacationFinder.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using VacationFinder.Services.Data;
    using VacationFinder.Web.ViewModels.Offer;

    public class OrderController : Controller
    {
        private readonly IOrderService orderService;

        public OrderController(
             IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SignUpToOfferViewModel viewModel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (this.ModelState.IsValid)
            {
                await this.orderService.CreateAsync(viewModel.Email, viewModel.OfferId, userId);
            }

            return this.RedirectToAction("Details", "Offer", new { id = viewModel.OfferId });
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int orderId)
        {
            await this.orderService.DeleteAsync(orderId);

            return this.RedirectToAction("ShowOrders", "User");
        }
    }
}
