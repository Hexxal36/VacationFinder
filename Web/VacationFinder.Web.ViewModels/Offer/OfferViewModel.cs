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
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Days { get; set; }

        public int Nights { get; set; }

        public double Price { get; set; }

        public Transport Transport { get; set; }

        public Tag Tag { get; set; }

        public Hotel Hotel { get; set; }

        public bool IsSpecial { get; set; }

        public IEnumerable<OfferImage> OfferImages { get; set; }
    }
}
