﻿namespace VacationFinder.Web.ViewModels.Administration.Tag
{
    using System.Collections.Generic;

    using VacationFinder.Data.Models;

    public class IndexViewModel
    {
        public IEnumerable<Tag> List { get; set; }

        public int Pages { get; set; }
    }
}
