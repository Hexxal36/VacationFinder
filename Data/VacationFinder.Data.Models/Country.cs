namespace VacationFinder.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using VacationFinder.Data.Common.Models;
    using VacationFinder.Data.Models.Enums;

    public class Country : BaseDeletableModel<int>
    {
        public Country()
        {
            this.Cities = new HashSet<City>();
        }

        [Required]
        [StringLength(30, MinimumLength = 10)]
        public string Name { get; set; }

        [Required]
        public Continent Continent { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}
