namespace VacationFinder.Services.Data.Tests.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;
    using VacationFinder.Data.Common.Repositories;
    using VacationFinder.Data.Models;
    using Xunit;

    public class CityServiceTests
    {
        [Fact]
        public void GetAllCitiesShouldReturnEveryCity()
        {
            var repository = this.CreateRepository();

            var service = new CityService(repository.Object);
            Assert.Equal(10, service.GetAllCities().Count());
            repository.Verify(x => x.All(), Times.Once);
            Assert.True(service.GetAllCities().ToList()[4].Name == "Varna");
        }

        [Fact]
        public void GetCityByIdShouldReturnTheCorrectCity()
        {
            var repository = this.CreateRepository();

            var service = new CityService(repository.Object);
            Assert.Equal(2, service.GetCityById(2).Id);
            Assert.Equal(4, service.GetCityById(4).Id);
        }

        [Fact]
        public void GetCityByIdShouldReturnNullWhenThereIsNoCityWithTheGivenId()
        {
            var repository = this.CreateRepository();

            var service = new CityService(repository.Object);
            Assert.Null(service.GetCityById(0));
            Assert.Null(service.GetCityById(-10));
            Assert.Null(service.GetCityById(1000));
        }

        private Mock<IDeletableEntityRepository<City>> CreateRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<City>>();
            repository.Setup(r => r.All()).Returns(new List<City>
            {
                new City() { Id = 1 },
                new City() { Id = 2 },
                new City() { Id = 3 },
                new City() { Id = 4 },
                new City
                {
                    Name = "Varna",
                    Country = new Country(),
                    Id = 5,
                },
                new City() { Id = 6 },
                new City() { Id = 7 },
                new City() { Id = 8 },
                new City() { Id = 9 },
                new City() { Id = 10 },
            }.AsQueryable());

            return repository;
        }
    }
}
