namespace VacationFinder.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Moq;
    using VacationFinder.Data.Common.Repositories;
    using VacationFinder.Data.Models;
    using Xunit;

    public class HotelServiceTests
    {
        private readonly HotelService service;
        private readonly Mock<IDeletableEntityRepository<Hotel>> repository;

        public HotelServiceTests()
        {
            var repository = new Mock<IDeletableEntityRepository<Hotel>>();
            repository.Setup(r => r.All()).Returns(new List<Hotel>
            {
                new Hotel
                {
                    Name = "test1",
                    Stars = 2,
                    CityId = 1,
                    Id = 1,
                    CreatedOn = DateTime.UtcNow,
                    IsActive = true,
                },
                new Hotel
                {
                    Name = "test2",
                    Stars = 2,
                    CityId = 2,
                    Id = 2,
                    CreatedOn = DateTime.UtcNow,
                    IsActive = true,
                },
                new Hotel
                {
                    Name = "test3",
                    Stars = 3,
                    CityId = 3,
                    Id = 3,
                    CreatedOn = DateTime.UtcNow,
                    IsActive = false,
                },
                new Hotel
                {
                    Name = "test4",
                    Stars = 3,
                    CityId = 4,
                    Id = 4,
                    CreatedOn = DateTime.UtcNow,
                    IsActive = true,
                },
            }.AsQueryable());

            this.repository = repository;
            this.service = new HotelService(repository.Object);
        }

        [Fact]
        public void GetAllHotelsShouldReturnAllHotels()
        {
            Assert.Equal(3, this.service.GetAllHotels().Count());
            this.repository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public void FilterHotelsShouldReturnOnlyTheHotelsThatHaveAllTheRequirements()
        {
            Assert.Equal(3, this.service.FilterHotels("test", -1, null).Count());
            Assert.Equal(2, this.service.FilterHotels(null, 2, null).Count());
            Assert.Single(this.service.FilterHotels(null, 3, null));
            Assert.Empty(this.service.FilterHotels(null, 5, null));
        }

        [Fact]
        public void GetCityByIdShouldReturnTheCorrectCity()
        {
            Assert.Equal(2, this.service.GetHotelById(2).Id);
            Assert.Equal(4, this.service.GetHotelById(4).Id);
        }

        [Fact]
        public void GetCityByIdShouldReturnNullWhenThereIsNoCityWithTheGivenId()
        {
            Assert.Throws<InvalidOperationException>(() => this.service.GetHotelById(0));
            Assert.Throws<InvalidOperationException>(() => this.service.GetHotelById(-10));
            Assert.Throws<InvalidOperationException>(() => this.service.GetHotelById(1000));
        }
    }
}
