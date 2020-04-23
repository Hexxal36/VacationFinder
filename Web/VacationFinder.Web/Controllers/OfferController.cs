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

        public IActionResult Details(int id)
        {
            try
            {
                Offer offer = this.offerService.GetOfferById(id);

                var viewModel = new OfferViewModel
                {
                    Offer = offer,
                    OfferImages = offer.Images,
                };

                return this.View(viewModel);
            }
            catch
            {
                return this.NotFound();
            }
        }
    }
}
