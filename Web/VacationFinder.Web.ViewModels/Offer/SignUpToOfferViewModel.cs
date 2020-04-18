namespace VacationFinder.Web.ViewModels.Offer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class SignUpToOfferViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int OfferId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Places { get; set; }
    }
}
