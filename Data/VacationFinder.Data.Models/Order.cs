namespace VacationFinder.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using VacationFinder.Data.Common.Models;

    public class Order : BaseDeletableModel<int>
    {
        [Required]
        public bool IsApproved { get; set; }

        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; }

        [Required]
        public int OfferId { get; set; }

        [ForeignKey("OfferId")]
        public virtual Offer Offer { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
