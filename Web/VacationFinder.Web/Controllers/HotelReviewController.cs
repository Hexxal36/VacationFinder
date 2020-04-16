namespace VacationFinder.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using VacationFinder.Data.Models;
    using VacationFinder.Services.Data;
    using VacationFinder.Web.ViewModels.Hotel;

    [AutoValidateAntiforgeryToken]
    public class HotelReviewController : Controller
    {
        private readonly IHotelReviewService hotelReviewService;

        public HotelReviewController(IHotelReviewService hotelReviewService)
        {
            this.hotelReviewService = hotelReviewService;
        }

        public async Task<IActionResult> Create(HotelReviewCreateViewModel viewModel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (this.ModelState.IsValid)
            {
                await this.hotelReviewService.CreateAsync(viewModel.Grade, viewModel.Title, viewModel.Body, viewModel.HotelId, userId);
            }

            return this.RedirectToAction("Details", "Hotel", new { id = viewModel.HotelId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(HotelReviewDeleteViewModel viewModel)
        {
            var review = this.hotelReviewService.GetReviewById(viewModel.ReviewId);
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (review.UserId == userId)
            {
                await this.hotelReviewService.DeleteAsync(viewModel.ReviewId);
            }

            return this.RedirectToAction("Details", "Hotel", new { id = viewModel.HotelId });
        }
    }
}
