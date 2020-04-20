namespace VacationFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using VacationFinder.Data.Common.Repositories;
    using VacationFinder.Data.Models;

    public class CityService : ICityService
    {
        private readonly IDeletableEntityRepository<City> _cityRepository;

        public CityService(
            IDeletableEntityRepository<City> cityRepository)
        {
            this._cityRepository = cityRepository;
        }

        public IEnumerable<City> GetAllCities()
        {
            List<City> query =
                this._cityRepository.All().OrderByDescending(x => x.CreatedOn).ToList();

            return query;
        }

        public City GetCityById(int id)
        {
            var cities = this._cityRepository.All().Where(x => x.Id == id).ToList();
            if (cities.Count == 0)
            {
                return null;
            }

            return cities.First();
        }
    }
}
