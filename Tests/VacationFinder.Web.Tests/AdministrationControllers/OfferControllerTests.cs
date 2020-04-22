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
    public class OfferControllerTests
    {
        private readonly ApplicationDbContext context;

        public OfferControllerTests()
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

            this.context.SaveChanges();
        }

        [Fact]
        [Priority(1)]
        public async Task IndexReturnsView()
        {
            var controller = new OfferController(this.context);

            var data = await controller.Index();

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(1)]
        public async Task CreateReturnsViewAsync()
        {
            var controller = new OfferController(this.context);

            var data = await controller.Create();

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(1)]
        public async Task CreateReturnsRedirectResultAndCreatesAnEntityInTheDB()
        {
            var controller = new OfferController(this.context);
            var currentOffers = this.context.Offers.ToList().Count;

            var offer = new Offer()
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
            };

            var createData = await controller.Create(offer);

            Assert.IsType<RedirectToActionResult>(createData);
            Assert.Equal(currentOffers + 1, this.context.Offers.ToList().Count);
        }

        [Fact]
        [Priority(1)]
        public async Task CreateThrowsExceptionWithInvalidData()
        {
            var controller = new OfferController(this.context);
            var currentOffers = this.context.Offers.ToList().Count;

            var offer = new Offer();

            await Assert.ThrowsAsync<DbUpdateException>(() => controller.Create(offer));
            Assert.Equal(currentOffers, this.context.Offers.ToList().Count);
        }

        [Fact]
        [Priority(2)]
        public async Task DetailsReturnViewResultWithExistingoffer()
        {
            var controller = new OfferController(this.context);
            var offerId = this.context.Offers.Where(x => x.Title == "TestingOffer").ToList().First().Id;

            var data = await controller.Details(offerId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task DetailsReturnNotFoundWithInvalidData()
        {
            var controller = new OfferController(this.context);

            var data = await controller.Details(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task EditReturnsViewWithExistingoffer()
        {
            var controller = new OfferController(this.context);
            var offerId = this.context.Offers.Where(x => x.Title == "TestingOffer").ToList().First().Id;

            var data = await controller.Edit(offerId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task EditReturnsNotFoundWithInvalidData()
        {
            var controller = new OfferController(this.context);

            var data = await controller.Edit(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(3)]
        public async Task EditReturnViewResultWithExistingEntry()
        {
            var controller = new OfferController(this.context);
            var offerId = this.context.Offers.Where(x => x.Title == "TestingOffer").ToList().First().Id;

            var offer = this.context.Offers.Where(x => x.Id == offerId).ToList().First();

            offer.Days = 2;

            var data = await controller.Edit(offerId, offer);

            Assert.IsType<RedirectToActionResult>(data);

            var editedoffer = this.context.Offers.Where(x => x.Id == offerId).ToList().First();

            Assert.Equal(2, editedoffer.Days);
        }

        [Fact]
        [Priority(3)]
        public async Task EditReturnNotFoundResultWithInvalidData()
        {
            var controller = new OfferController(this.context);

            var data = await controller.Edit(-1, new Offer());

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteReturnsDeleteViewWithValidData()
        {
            var controller = new OfferController(this.context);
            var transportId = this.context.Offers.Where(x => x.Title == "TestingOffer").ToList().First().Id;

            var data = await controller.Delete(transportId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteReturnsDeleteViewWithInvalidData()
        {
            var controller = new OfferController(this.context);

            var data = await controller.Delete(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteShouldReturnRedirectResult()
        {
            var controller = new OfferController(this.context);
            var offerId = this.context.Offers.Where(x => x.Title == "TestingOffer").ToList().First().Id;

            var data = await controller.DeleteConfirmed(offerId);

            Assert.IsType<RedirectToActionResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteShouldThrowIfIdIsInvalid()
        {
            var controller = new OfferController(this.context);

            await Assert.ThrowsAsync<NullReferenceException>(() => controller.DeleteConfirmed(-1));
        }
    }
}
