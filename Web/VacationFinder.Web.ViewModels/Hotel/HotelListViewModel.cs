namespace VacationFinder.Web.ViewModels.Hotel
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using VacationFinder.Data.Models;

    public class HotelListViewModel
    {
        public IEnumerable<Hotel> Hotels { get; set; }

        public IEnumerable<City> Cities { get; set; }
    }
}
