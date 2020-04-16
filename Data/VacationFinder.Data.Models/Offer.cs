namespace VacationFinder.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using VacationFinder.Data.Common.Models;
    using VacationFinder.Data.Models.Enums;

    public class Offer : BaseDeletableModel<int>
    {
        public Offer()
        {
            this.Images = new HashSet<OfferImage>();
        }

        [Required]
        [StringLength(30, MinimumLength =10)]
        public string Title { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int Days { get; set; }

        [Required]
        public int Nights { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool IsSpecial { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int HotelId { get; set; }

        [ForeignKey("HotelId")]
        public virtual Hotel Hotel { get; set; }

        [Required]
        public int TagId { get; set; }

        [ForeignKey("TagId")]
        public virtual Tag Tag { get; set; }

        [Required]
        public int TransportId { get; set; }

        [ForeignKey("TransportId")]
        public virtual Transport Transport { get; set; }

        public virtual ICollection<OfferImage> Images { get; set; }
    }
}
