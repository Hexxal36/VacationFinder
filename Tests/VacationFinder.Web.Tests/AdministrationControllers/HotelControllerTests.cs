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
    public class HotelControllerTests
    {
        private readonly ApplicationDbContext context;

        public HotelControllerTests()
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

            if (this.context.Cities.Count() == 0)
            {
                this.context.Cities.Add(new City()
                {
                    Name = "TestingCity",
                    CountryId = this.context.Countries.First().Id,
                });
            }

            this.context.SaveChanges();
        }

        [Fact]
        [Priority(1)]
        public async Task IndexReturnsView()
        {
            var controller = new HotelController(this.context);

            var data = await controller.Index();

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(1)]
        public async Task CreateReturnsViewAsync()
        {
            var controller = new HotelController(this.context);

            var data = await controller.Create();

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(1)]
        public async Task CreateReturnsRedirectResultAndCreatesAnEntityInTheDB()
        {
            var controller = new HotelController(this.context);
            var currentHotels = this.context.Hotels.ToList().Count;

            var hotel = new Hotel()
            {
                Name = "TestingHotel",
                CityId = this.context.Cities.OrderBy(x => x.Id).First().Id,
                ImageUrl = "https://media.sproutsocial.com/uploads/2017/02/10x-featured-social-media-image-size.png",
                Stars = 4,
                Description = "test",
            };

            var createData = await controller.Create(hotel);

            Assert.IsType<RedirectToActionResult>(createData);
            Assert.Equal(currentHotels + 1, this.context.Hotels.ToList().Count);
        }

        [Fact]
        [Priority(1)]
        public async Task CreateThrowsExceptionWithInvalidData()
        {
            var controller = new HotelController(this.context);
            var currentHotels = this.context.Hotels.ToList().Count;

            var hotel = new Hotel();

            await Assert.ThrowsAsync<DbUpdateException>(() => controller.Create(hotel));
            Assert.Equal(currentHotels, this.context.Hotels.ToList().Count);
        }

        [Fact]
        [Priority(2)]
        public async Task DetailsReturnViewResultWithExistingEntry()
        {
            var controller = new HotelController(this.context);
            var hotelId = this.context.Hotels.Where(x => x.Name == "TestingHotel").ToList().First().Id;

            var data = await controller.Details(hotelId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task DetailsReturnNotFoundWithInvalidData()
        {
            var controller = new HotelController(this.context);

            var data = await controller.Details(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task EditReturnsViewWithExistingHotel()
        {
            var controller = new HotelController(this.context);
            var hotelId = this.context.Hotels.Where(x => x.Name == "TestingHotel").ToList().First().Id;

            var data = await controller.Edit(hotelId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task EditReturnsNotFoundWithInvalidData()
        {
            var controller = new HotelController(this.context);

            var data = await controller.Edit(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(3)]
        public async Task EditReturnViewResultWithExistingHotel()
        {
            var controller = new HotelController(this.context);
            var hotelId = this.context.Hotels.Where(x => x.Name == "TestingHotel").ToList().First().Id;

            var hotel = this.context.Hotels.Where(x => x.Id == hotelId).ToList().First();

            hotel.CityId = this.context.Cities.OrderBy(x => x.Id).Last().Id;

            var data = await controller.Edit(hotelId, hotel);

            Assert.IsType<RedirectToActionResult>(data);

            var editedHotel = this.context.Hotels.Where(x => x.Id == hotelId).ToList().First();

            if (this.context.Cities.Count() >= 2)
            {
                Assert.NotEqual(
                  this.context.Cities.OrderBy(x => x.Id).First().Id,
                  editedHotel.CityId);
            }
        }

        [Fact]
        [Priority(3)]
        public async Task EditReturnNotFoundResultWithInvalidData()
        {
            var controller = new HotelController(this.context);

            var data = await controller.Edit(-1, new Hotel());

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteReturnsDeleteViewWithValidData()
        {
            var controller = new HotelController(this.context);
            var hotelId = this.context.Hotels.Where(x => x.Name == "TestingHotel").ToList().First().Id;

            var data = await controller.Delete(hotelId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteReturnsDeleteViewWithInvalidData()
        {
            var controller = new HotelController(this.context);

            var data = await controller.Delete(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteShouldReturnRedirectResult()
        {
            var controller = new HotelController(this.context);
            var hotelId = this.context.Hotels.Where(x => x.Name == "TestingHotel").ToList().First().Id;

            var data = await controller.DeleteConfirmed(hotelId);

            Assert.IsType<RedirectToActionResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteShouldThrowIfIdIsInvalid()
        {
            var controller = new HotelController(this.context);

            await Assert.ThrowsAsync<NullReferenceException>(() => controller.DeleteConfirmed(-1));
        }
    }
}
