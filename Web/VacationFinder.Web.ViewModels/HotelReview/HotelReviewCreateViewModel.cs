namespace VacationFinder.Web.ViewModels.Hotel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using VacationFinder.Data.Models;

    public class HotelReviewCreateViewModel
    {
        [Range(0, 5)]
        public int Grade { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 10)]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        public int HotelId { get; set; }
    }
}
