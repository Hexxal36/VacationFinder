namespace VacationFinder.Web.ViewModels.Administration.Country
{
    using System.Collections.Generic;

    using VacationFinder.Data.Models;

    public class IndexViewModel
    {
        public IEnumerable<Country> List { get; set; }

        public int Pages { get; set; }
    }
}
