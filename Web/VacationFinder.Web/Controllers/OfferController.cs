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
        private readonly IOfferService _offerService;

        public OfferController(
             IOfferService offerService)
        {
            this._offerService = offerService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Details(int id)
        {
            Offer offer = this._offerService.GetOfferById(id);

            var viewModel = new OfferViewModel
            {
                Offer = offer,
                OfferImages = offer.Images,
            };

            return this.View(viewModel);
        }
    }
}
