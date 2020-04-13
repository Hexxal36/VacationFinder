namespace VacationFinder.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using VacationFinder.Data.Common.Models;

    public class OfferImage : BaseDeletableModel<int>
    {
        [Required]
        [Url]
        public string ImageUrl { get; set; }

        [Required]
        public int OfferId { get; set; }

        [ForeignKey("OfferId")]
        public virtual Offer Offer { get; set; }
    }
}
