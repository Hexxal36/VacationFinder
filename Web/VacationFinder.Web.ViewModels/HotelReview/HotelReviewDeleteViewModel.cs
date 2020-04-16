namespace VacationFinder.Web.ViewModels.Hotel
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using VacationFinder.Data.Models;

    public class HotelReviewDeleteViewModel
    {
        public int ReviewId { get; set; }

        public int HotelId { get; set; }
    }
}
