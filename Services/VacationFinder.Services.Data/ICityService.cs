namespace VacationFinder.Services.Data
{
    using System.Collections.Generic;

    using VacationFinder.Data.Models;

    public interface ICityService
    {
        IEnumerable<City> GetAllCities();

        City GetCityById(int id);
    }
}
