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
        private readonly IOrderService _orderService;
        private readonly IOfferService _offerService;

        public OrderController(
             IOrderService orderService,
             IOfferService offerService)
        {
            this._orderService = orderService;
            this._offerService = offerService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SignUpToOfferViewModel viewModel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var offer = this._offerService.GetOfferById(viewModel.OfferId);

            if (this.ModelState.IsValid &&
                offer.Places > 0 &&
                viewModel.Places > 0 &&
                offer.Places >= viewModel.Places)
            {
                var order = await this._orderService.CreateAsync(viewModel.Email, viewModel.Places, viewModel.OfferId, userId);
                await this._offerService.OnOrder(viewModel.OfferId, order.Id);
            }

            return this.RedirectToAction("Details", "Offer", new { id = viewModel.OfferId });
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int orderId)
        {
            var order = this._orderService.GetOrderById(orderId);

            await this._offerService.OnOrderDelete(order.OfferId, orderId);

            await this._orderService.DeleteAsync(orderId);

            return this.RedirectToAction("ShowOrders", "User");
        }
    }
}
