namespace VacationFinder.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;
    using VacationFinder.Data.Common.Repositories;
    using VacationFinder.Data.Models;
    using Xunit;

    public class CityServiceTests
    {
        private readonly CityService service;
        private readonly Mock<IDeletableEntityRepository<City>> repository;

        public CityServiceTests()
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

            this.repository = repository;
            this.service = new CityService(repository.Object);
        }

        [Fact]
        public void GetAllCitiesShouldReturnEveryCity()
        {
            Assert.Equal(10, this.service.GetAllCities().Count());
            this.repository.Verify(x => x.All(), Times.Once);
            Assert.True(this.service.GetAllCities().ToList()[4].Name == "Varna");
        }

        [Fact]
        public void GetCityByIdShouldReturnTheCorrectCity()
        {
            Assert.Equal(2, this.service.GetCityById(2).Id);
            Assert.Equal(4, this.service.GetCityById(4).Id);
        }

        [Fact]
        public void GetCityByIdShouldReturnNullWhenThereIsNoCityWithTheGivenId()
        {
            Assert.Null(this.service.GetCityById(0));
            Assert.Null(this.service.GetCityById(-10));
            Assert.Null(this.service.GetCityById(1000));
        }
    }
}
