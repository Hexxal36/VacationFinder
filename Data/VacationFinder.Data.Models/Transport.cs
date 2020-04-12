namespace VacationFinder.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using VacationFinder.Data.Common.Models;

    public class Transport : BaseDeletableModel<int>
    {
        public Transport()
        {
            this.Transports = new HashSet<Transport>();
        }

        [Required]
        public string Title { get; set; }

        [Required]
        public int Sort { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Transport> Transports { get; set; }
    }
}
