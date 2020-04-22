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
    public class CountryControllerTests
    {
        private readonly ApplicationDbContext context;

        public CountryControllerTests()
        {
            this.context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("Server=.;Database=VacationFinder;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options);
        }

        [Fact]
        [Priority(1)]
        public async Task IndexReturnsView()
        {
            var controller = new CountryController(this.context);

            var data = await controller.Index();

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(1)]
        public void CreateReturnsView()
        {
            var controller = new CountryController(this.context);
            Assert.IsType<ViewResult>(controller.Create());
        }

        [Fact]
        [Priority(1)]
        public async Task CreateReturnsRedirectResultAndCreatesAnEntityInTheDB()
        {
            var controller = new CountryController(this.context);
            var currentCountries = this.context.Countries.ToList().Count;

            var country = new Country()
            {
                Name = "TestingCountry",
                Continent = Continent.Unknown,
            };

            var createData = await controller.Create(country);

            Assert.IsType<RedirectToActionResult>(createData);
            Assert.Equal(currentCountries + 1, this.context.Countries.ToList().Count);
        }

        [Fact]
        [Priority(1)]
        public async Task CreateThrowsExceptionWithInvalidData()
        {
            var controller = new CountryController(this.context);
            var currentCountries = this.context.Countries.ToList().Count;

            var country = new Country();

            await Assert.ThrowsAsync<DbUpdateException>(() => controller.Create(country));
            Assert.Equal(currentCountries, this.context.Countries.ToList().Count);
        }

        [Fact]
        [Priority(2)]
        public async Task DetailsReturnViewResultWithExistingEntry()
        {
            var controller = new CountryController(this.context);
            var countryId = this.context.Countries.Where(x => x.Name == "TestingCountry").ToList().First().Id;

            var data = await controller.Details(countryId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task DetailsReturnNotFoundWithInvalidData()
        {
            var controller = new CountryController(this.context);

            var data = await controller.Details(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task EditReturnsViewWithExistingCountry()
        {
            var controller = new CountryController(this.context);
            var countryId = this.context.Countries.Where(x => x.Name == "TestingCountry").ToList().First().Id;

            var data = await controller.Edit(countryId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task EditReturnsNotFoundWithInvalidData()
        {
            var controller = new CountryController(this.context);

            var data = await controller.Edit(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(3)]
        public async Task EditReturnViewResultWithExistingCountry()
        {
            var controller = new CountryController(this.context);
            var countryId = this.context.Countries.Where(x => x.Name == "TestingCountry").ToList().First().Id;

            var country = this.context.Countries.Where(x => x.Id == countryId).ToList().First();

            country.Continent = Continent.Europe;

            var data = await controller.Edit(countryId, country);

            Assert.IsType<RedirectToActionResult>(data);

            var editedCountry = this.context.Countries.Where(x => x.Id == countryId).ToList().First();

            Assert.Equal(Continent.Europe, editedCountry.Continent);
        }

        [Fact]
        [Priority(3)]
        public async Task EditReturnNotFoundResultWithInvalidData()
        {
            var controller = new CountryController(this.context);

            var data = await controller.Edit(-1, new Country());

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteReturnsDeleteViewWithValidData()
        {
            var controller = new CountryController(this.context);
            var transportId = this.context.Countries.Where(x => x.Name == "TestingCountry").ToList().First().Id;

            var data = await controller.Delete(transportId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteReturnsDeleteViewWithInvalidData()
        {
            var controller = new CountryController(this.context);

            var data = await controller.Delete(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteShouldReturnRedirectResult()
        {
            var controller = new CountryController(this.context);
            var countryId = this.context.Countries.Where(x => x.Name == "TestingCountry").ToList().First().Id;

            var data = await controller.DeleteConfirmed(countryId);

            Assert.IsType<RedirectToActionResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteShouldThrowIfIdIsInvalid()
        {
            var controller = new CountryController(this.context);

            await Assert.ThrowsAsync<NullReferenceException>(() => controller.DeleteConfirmed(-1));
        }
    }
}
