namespace VacationFinder.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using VacationFinder.Data.Models;
    using VacationFinder.Services.Data;
    using VacationFinder.Web.ViewModels.Offer;

    public class OfferController : Controller
    {
        private readonly IOfferService offerService;

        public OfferController(
             IOfferService offerService)
        {
            this.offerService = offerService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Details(int id)
        {
            Offer offer = this.offerService.GetOfferById(id);

            var viewModel = new OfferViewModel
            {
                Title = offer.Title,
                Days = offer.Days,
                Description = offer.Description,
                Hotel = offer.Hotel,
                Id = offer.Id,
                IsSpecial = offer.IsSpecial,
                Nights = offer.Nights,
                Price = offer.Price,
                Tag = offer.Tag,
                Transport = offer.Transport,
                OfferImages = offer.Images,
            };

            return this.View(viewModel);
        }
    }
}
