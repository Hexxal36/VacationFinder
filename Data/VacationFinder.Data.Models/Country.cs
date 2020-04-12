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
            this.Countries = new HashSet<Country>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public Continent Continent { get; set; }

        public virtual ICollection<Country> Countries { get; set; }
    }
}
