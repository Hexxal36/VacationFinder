namespace VacationFinder.Web.Tests.AdministrationControllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using VacationFinder.Data;
    using VacationFinder.Data.Models;
    using VacationFinder.Data.Models.Enums;
    using VacationFinder.Web.Areas.Administration.Controllers;
    using Xunit;
    using Xunit.Priority;

    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class CityControllerTests
    {
        private readonly ApplicationDbContext context;

        public CityControllerTests()
        {
            this.context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("Server=.;Database=VacationFinder;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options);

            if (this.context.Countries.Count() == 0)
            {
                this.context.Countries.Add(new Country()
                {
                    Name = "TestingCountry",
                    Continent = Continent.Unknown,
                });
            }

            this.context.SaveChanges();
        }

        [Fact]
        [Priority(1)]
        public async Task IndexReturnsView()
        {
            var controller = new CityController(this.context);

            var data = await controller.Index();

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(1)]
        public async Task CreateReturnsViewAsync()
        {
            var controller = new CityController(this.context);

            var data = await controller.Create();

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(1)]
        public async Task CreateReturnsRedirectResultAndCreatesAnEntityInTheDB()
        {
            var controller = new CityController(this.context);
            var currentCities = this.context.Cities.ToList().Count;

            var city = new City()
            {
                Name = "TestingCity",
                CountryId = this.context.Countries.OrderBy(x => x.Id).First().Id,
            };

            var createData = await controller.Create(city);

            Assert.Equal(currentCities + 1, this.context.Cities.ToList().Count);
            Assert.IsType<RedirectToActionResult>(createData);
        }

        [Fact]
        [Priority(1)]
        public async Task CreateThrowsExceptionWithInvalidData()
        {
            var controller = new CityController(this.context);
            var currentCities = this.context.Cities.ToList().Count;

            var city = new City();

            await Assert.ThrowsAsync<DbUpdateException>(() => controller.Create(city));
            Assert.Equal(currentCities, this.context.Cities.ToList().Count);
        }

        [Fact]
        [Priority(2)]
        public async Task DetailsReturnViewResultWithExistingEntry()
        {
            var controller = new CityController(this.context);
            var cityId = this.context.Cities.Where(x => x.Name == "TestingCity").ToList().First().Id;

            var data = await controller.Details(cityId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task DetailsReturnNotFoundWithInvalidData()
        {
            var controller = new CityController(this.context);

            var data = await controller.Details(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task EditReturnsViewWithExistingCity()
        {
            var controller = new CityController(this.context);
            var cityId = this.context.Cities.Where(x => x.Name == "TestingCity").ToList().First().Id;

            var data = await controller.Edit(cityId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task EditReturnsNotFoundWithInvalidData()
        {
            var controller = new CityController(this.context);

            var data = await controller.Edit(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(3)]
        public async Task EditReturnViewResultWithExistingCity()
        {
            var controller = new CityController(this.context);
            var cityId = this.context.Cities.Where(x => x.Name == "TestingCity").ToList().First().Id;

            var city = this.context.Cities.Where(x => x.Id == cityId).ToList().First();

            city.CountryId = this.context.Countries.OrderBy(x => x.Id).Last().Id;

            var data = await controller.Edit(cityId, city);

            Assert.IsType<RedirectToActionResult>(data);

            var editedCity = this.context.Cities.Where(x => x.Id == cityId).ToList().First();

            Assert.NotEqual(
                this.context.Countries.OrderBy(x => x.Id).First().Id,
                editedCity.CountryId);
        }

        [Fact]
        [Priority(3)]
        public async Task EditReturnNotFoundResultWithInvalidData()
        {
            var controller = new CityController(this.context);

            var data = await controller.Edit(-1, new City());

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteReturnsDeleteViewWithValidData()
        {
            var controller = new CityController(this.context);
            var cityId = this.context.Cities.Where(x => x.Name == "TestingCity").ToList().First().Id;

            var data = await controller.Delete(cityId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteReturnsDeleteViewWithInvalidData()
        {
            var controller = new CityController(this.context);

            var data = await controller.Delete(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteShouldReturnRedirectResult()
        {
            var controller = new CityController(this.context);
            var cityId = this.context.Cities.Where(x => x.Name == "TestingCity").ToList().First().Id;

            var data = await controller.DeleteConfirmed(cityId);

            Assert.IsType<RedirectToActionResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteShouldThrowIfIdIsInvalid()
        {
            var controller = new CityController(this.context);

            await Assert.ThrowsAsync<NullReferenceException>(() => controller.DeleteConfirmed(-1));
        }
    }
}
