namespace VacationFinder.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using VacationFinder.Data.Common.Models;
    using VacationFinder.Data.Models.Enums;

    public class City : BaseDeletableModel<int>
    {
        public City()
        {
            this.Hotels = new HashSet<Hotel>();
        }

        [Required]
        [StringLength(30, MinimumLength = 10)]
        public string Name { get; set; }

        [Required]
        public int CountryId { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        public virtual ICollection<Hotel> Hotels { get; set; }
    }
}
