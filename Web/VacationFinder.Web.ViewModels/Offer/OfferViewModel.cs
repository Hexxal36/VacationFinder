namespace VacationFinder.Web.ViewModels.Offer
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using AutoMapper;
    using VacationFinder.Data.Models;
    using VacationFinder.Services.Mapping;

    public class OfferViewModel : IMapFrom<Offer>, IMapTo<Offer>
    {
        public Offer Offer { get; set; }

        public IEnumerable<OfferImage> OfferImages { get; set; }
    }
}
