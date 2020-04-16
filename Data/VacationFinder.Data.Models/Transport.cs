namespace VacationFinder.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using VacationFinder.Data.Common.Models;

    public class Transport : BaseDeletableModel<int>
    {
        public Transport()
        {
            this.Offers = new HashSet<Offer>();
        }

        [Required]
        [StringLength(30, MinimumLength = 10)]
        public string Title { get; set; }

        [Required]
        public int Sort { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual ICollection<Offer> Offers { get; set; }
    }
}
