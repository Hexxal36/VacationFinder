namespace VacationFinder.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using VacationFinder.Data.Common.Models;

    public class Transport : BaseDeletableModel<int>
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public int Sort { get; set; }

        public bool IsActive { get; set; }
    }
}
