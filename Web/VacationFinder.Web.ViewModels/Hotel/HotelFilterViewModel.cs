namespace VacationFinder.Web.ViewModels.Hotel
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using VacationFinder.Data.Models;

    public class HotelFilterViewModel
    {
        public HotelFilterViewModel()
        {
            this.Stars = -1;
        }

        public string Name { get; set; }

        public int Stars { get; set; }

        public City City { get; set; }
    }
}
