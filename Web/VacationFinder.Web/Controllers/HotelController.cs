namespace VacationFinder.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using VacationFinder.Data.Models;
    using VacationFinder.Services.Data;
    using VacationFinder.Web.ViewModels.Hotel;

    public class HotelController : Controller
    {
        private readonly IHotelService hotelService;
        private readonly ICityService cityService;
        private readonly IHotelReviewService hotelReviewService;

        public HotelController(
             IHotelService hotelService,
             ICityService cityService,
             IHotelReviewService hotelReviewService)
        {
            this.hotelService = hotelService;
            this.cityService = cityService;
            this.hotelReviewService = hotelReviewService;
        }

        public IActionResult Index(HotelFilterViewModel filter)
        {
            var hotels = this.hotelService.GetAllHotels();
            var cities = this.cityService.GetAllCities();

            var viewModel = new HotelListViewModel
            {
                Hotels = hotels,
                Cities = cities,
            };

            viewModel.Hotels = this.hotelService.FilterHotels(hotels, filter.Name, filter.Stars, filter.City);

            return this.View(viewModel);
        }

        public IActionResult Details(int id)
        {
            var hotel = this.hotelService.GetHotelById(id);
            var reviews = hotel.HotelReviews;

            var viewModel = new HotelViewModel
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Stars = hotel.Stars,
                Description = hotel.Description,
                ImageUrl = hotel.ImageUrl,
                Offers = hotel.Offers.ToList(),
                HotelReviews = reviews,
            };

            return this.View(viewModel);
        }
    }
}
