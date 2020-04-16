namespace VacationFinder.Web.ViewModels.Hotel
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using VacationFinder.Data.Models;

    public class HotelViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Stars { get; set; }

        public string ImageUrl { get; set; }

        public IEnumerable<Offer> Offers { get; set; }
    }
}
