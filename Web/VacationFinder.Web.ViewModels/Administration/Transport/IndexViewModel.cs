namespace VacationFinder.Web.ViewModels.Administration.Transport
{
    using System.Collections.Generic;

    using VacationFinder.Data.Models;

    public class IndexViewModel
    {
        public IEnumerable<Transport> List { get; set; }

        public int Pages { get; set; }
    }
}