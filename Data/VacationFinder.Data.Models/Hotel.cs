namespace VacationFinder.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using VacationFinder.Data.Common.Models;
    using VacationFinder.Data.Models.Enums;

    public class Hotel : BaseDeletableModel<int>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [Range(0, 5)]
        public int Stars { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int CityId { get; set; }

        [ForeignKey("CityId")]
        public virtual City City { get; set; }
    }
}
