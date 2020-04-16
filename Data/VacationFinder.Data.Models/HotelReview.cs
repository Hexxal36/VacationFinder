namespace VacationFinder.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using VacationFinder.Data.Common.Models;

    public class HotelReview : BaseDeletableModel<int>
    {
        [Required]
        [Range(0, 5)]
        public int Grade { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 10)]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public int HotelId { get; set; }

        [ForeignKey("HotelId")]
        public virtual Hotel Hotel { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
