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
        private readonly IOfferService offerService;

        public OrderController(
             IOrderService orderService,
             IOfferService offerService)
        {
            this.orderService = orderService;
            this.offerService = offerService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SignUpToOfferViewModel viewModel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var offer = this.offerService.GetOfferById(viewModel.OfferId);

            if (this.ModelState.IsValid &&
                offer.Places > 0 &&
                viewModel.Places > 0 &&
                offer.Places >= viewModel.Places)
            {
                var order = await this.orderService.CreateAsync(viewModel.Email, viewModel.Places, viewModel.OfferId, userId);
                await this.offerService.OnOrderAsync(viewModel.OfferId, order.Id);
            }

            return this.RedirectToAction("Details", "Offer", new { id = viewModel.OfferId });
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int orderId)
        {
            var order = this.orderService.GetOrderById(orderId);

            await this.offerService.OnOrderDeleteAsync(order.OfferId, orderId);

            await this.orderService.DeleteAsync(orderId);

            return this.RedirectToAction("ShowOrders", "User");
        }
    }
}
