namespace VacationFinder.Services.Data.Tests.Services
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
        [Fact]
        public void GetAllHotelsShouldReturnAllHotels()
        {
            var repository = this.CreateRepository();
            var service = new HotelService(repository.Object);

            Assert.Equal(3, service.GetAllHotels().Count());
            repository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public void FilterHotelsShouldReturnOnlyTheHotelsThatHaveAllTheRequirements()
        {
            var repository = this.CreateRepository();
            var service = new HotelService(repository.Object);

            Assert.Equal(3, service.FilterHotels("test", -1, null).Count());
            Assert.Equal(2, service.FilterHotels(null, 2, null).Count());
            Assert.Single(service.FilterHotels(null, 3, null));
            Assert.Empty(service.FilterHotels(null, 5, null));
        }

        [Fact]
        public void GetCityByIdShouldReturnTheCorrectCity()
        {
            var repository = this.CreateRepository();
            var service = new HotelService(repository.Object);

            Assert.Equal(2, service.GetHotelById(2).Id);
            Assert.Equal(4, service.GetHotelById(4).Id);
        }

        [Fact]
        public void GetCityByIdShouldReturnNullWhenThereIsNoCityWithTheGivenId()
        {
            var repository = this.CreateRepository();

            var service = new HotelService(repository.Object);

            Assert.Throws<InvalidOperationException>(() => service.GetHotelById(0));
            Assert.Throws<InvalidOperationException>(() => service.GetHotelById(-10));
            Assert.Throws<InvalidOperationException>(() => service.GetHotelById(1000));
        }

        private Mock<IDeletableEntityRepository<Hotel>> CreateRepository()
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

            return repository;
        }
    }
}
