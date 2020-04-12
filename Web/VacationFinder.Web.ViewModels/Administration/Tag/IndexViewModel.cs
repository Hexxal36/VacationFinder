namespace VacationFinder.Web.ViewModels.Administration.Tag
{
    using VacationFinder.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class IndexViewModel
    {
        public IEnumerable<Tag> List { get; set; }

        public int Pages { get; set; }
    }
}
