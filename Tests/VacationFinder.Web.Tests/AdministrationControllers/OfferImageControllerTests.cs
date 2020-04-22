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
    public class OfferImageControllerTests
    {
        private readonly ApplicationDbContext context;
        private readonly string testImageUrl = "https://p.bigstockphoto.com/GeFvQkBbSLaMdpKXF1Zv_bigstock-Aerial-View-Of-Blue-Lakes-And--227291596.jpg";

        public OfferImageControllerTests()
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

            if (this.context.Hotels.Count() == 0)
            {
                this.context.Hotels.Add(new Hotel()
                {
                    Name = "TestingHotel",
                    CityId = this.context.Cities.OrderBy(x => x.Id).First().Id,
                    ImageUrl = "https://media.sproutsocial.com/uploads/2017/02/10x-featured-social-media-image-size.png",
                    Stars = 4,
                    Description = "test",
                });
            }

            if (this.context.Tags.Count() == 0)
            {
                this.context.Tags.Add(new Tag()
                {
                    Title = "TestingTag",
                    Sort = 1,
                    IsActive = true,
                });
            }

            if (this.context.Transports.Count() == 0)
            {
                this.context.Transports.Add(new Transport()
                {
                    Sort = 1,
                    Title = "TestingTransport",
                    IsActive = true,
                });
            }

            if (this.context.Offers.Count() == 0)
            {
                this.context.Offers.Add(new Offer()
                {
                    Title = "TestingOffer",
                    Days = 1,
                    Nights = 1,
                    Places = 1,
                    Price = 1,
                    TagId = this.context.Tags.First().Id,
                    HotelId = this.context.Hotels.First().Id,
                    TransportId = this.context.Transports.First().Id,
                    Description = "test",
                    IsActive = true,
                });
            }

            this.context.SaveChanges();
        }

        [Fact]
        [Priority(1)]
        public async Task CreateReturnsViewAsync()
        {
            var controller = new OfferImageController(this.context);

            var data = await controller.Create();

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(1)]
        public async Task CreateReturnsRedirectResultAndCreatesAnEntityInTheDB()
        {
            var controller = new OfferImageController(this.context);
            var currentImages = this.context.OfferImages.ToList().Count;

            var offerImage = new OfferImage()
            {
                ImageUrl = this.testImageUrl,
                OfferId = this.context.Offers.First().Id,
            };

            var createData = await controller.Create(offerImage);

            Assert.IsType<RedirectToActionResult>(createData);
            Assert.Equal(currentImages + 1, this.context.OfferImages.ToList().Count);
        }

        [Fact]
        [Priority(1)]
        public async Task CreateThrowsExceptionWithInvalidData()
        {
            var controller = new OfferImageController(this.context);
            var currentImages = this.context.OfferImages.ToList().Count;

            var offerImage = new OfferImage()
            {
                ImageUrl = null,
                OfferId = this.context.Offers.First().Id,
            };

            await Assert.ThrowsAsync<DbUpdateException>(() => controller.Create(offerImage));
            Assert.Equal(currentImages, this.context.OfferImages.ToList().Count);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteReturnsDeleteViewWithValidData()
        {
            var controller = new OfferImageController(this.context);
            var imageId = this.context.OfferImages.Where(x => x.ImageUrl == this.testImageUrl).ToList().First().Id;

            var data = await controller.Delete(imageId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteReturnsDeleteViewWithInvalidData()
        {
            var controller = new OfferImageController(this.context);

            var data = await controller.Delete(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteShouldReturnRedirectResult()
        {
            var controller = new OfferImageController(this.context);
            var offerImageId = this.context.OfferImages.Where(x => x.ImageUrl == this.testImageUrl).ToList().First().Id;

            var data = await controller.DeleteConfirmed(offerImageId);

            Assert.IsType<RedirectToActionResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteShouldThrowIfIdIsInvalid()
        {
            var controller = new OfferImageController(this.context);

            await Assert.ThrowsAsync<NullReferenceException>(() => controller.DeleteConfirmed(-1));
        }
    }
}
