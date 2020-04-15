namespace VacationFinder.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using VacationFinder.Data.Models;

    public class IndexViewModel
    {
        public IEnumerable<Offer> SpecialOffers { get; set; }
    }
}
