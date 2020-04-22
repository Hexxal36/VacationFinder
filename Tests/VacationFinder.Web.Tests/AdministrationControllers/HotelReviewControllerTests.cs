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
    using VacationFinder.Data.Seeding;
    using VacationFinder.Web.Areas.Administration.Controllers;
    using Xunit;
    using Xunit.Priority;

    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class HotelReviewControllerTests
    {
        private readonly ApplicationDbContext context;

        public HotelReviewControllerTests()
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

            if (this.context.HotelReviews.Where(x => x.Title == "TestingHotelReview").Count() == 0)
            {
                this.context.HotelReviews.Add(new HotelReview()
                {
                    Title = "TestingHotelReview",
                    Body = "Test",
                    Grade = 4,
                    HotelId = this.context.Hotels.First().Id,
                    UserId = this.context.Users.First().Id,
                });
            }

            this.context.SaveChanges();
        }

        [Fact]
        [Priority(1)]
        public async Task IndexReturnsView()
        {
            var controller = new HotelReviewController(this.context);

            var data = await controller.Index();

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task DetailsReturnViewResultWithExistingTag()
        {
            var controller = new HotelReviewController(this.context);
            var reviewId = this.context.HotelReviews.Where(x => x.Title == "TestingHotelReview").ToList().First().Id;

            var data = await controller.Details(reviewId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task DetailsReturnNotFoundWithInvalidData()
        {
            var controller = new HotelReviewController(this.context);

            var data = await controller.Details(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task EditReturnsViewWithExistingTag()
        {
            var controller = new HotelReviewController(this.context);
            var reviewId = this.context.HotelReviews.Where(x => x.Title == "TestingHotelReview").ToList().First().Id;

            var data = await controller.Edit(reviewId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task EditReturnsNotFoundWithInvalidData()
        {
            var controller = new HotelReviewController(this.context);

            var data = await controller.Edit(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(3)]
        public async Task EditReturnViewResultWithExistingEntry()
        {
            var controller = new HotelReviewController(this.context);
            var reviewId = this.context.HotelReviews.Where(x => x.Title == "TestingHotelReview").ToList().First().Id;

            var review = this.context.HotelReviews.Where(x => x.Id == reviewId).ToList().First();

            Assert.Equal(4, review.Grade);

            review.Grade = 3;

            var data = await controller.Edit(reviewId, review);

            Assert.IsType<RedirectToActionResult>(data);

            var editedReview = this.context.HotelReviews.Where(x => x.Id == reviewId).ToList().First();

            Assert.Equal(3, editedReview.Grade);
        }

        [Fact]
        [Priority(3)]
        public async Task EditReturnNotFoundResultWithInvalidData()
        {
            var controller = new HotelReviewController(this.context);

            var data = await controller.Edit(-1, new HotelReview());

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteReturnsDeleteViewWithValidData()
        {
            var controller = new HotelReviewController(this.context);
            var reviewId = this.context.HotelReviews.Where(x => x.Title == "TestingHotelReview").ToList().First().Id;

            var data = await controller.Delete(reviewId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteReturnsDeleteViewWithInvalidData()
        {
            var controller = new HotelReviewController(this.context);

            var data = await controller.Delete(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteShouldReturnRedirectResult()
        {
            var controller = new HotelReviewController(this.context);
            var reviewId = this.context.HotelReviews.Where(x => x.Title == "TestingHotelReview").ToList().First().Id;

            var data = await controller.DeleteConfirmed(reviewId);

            Assert.IsType<RedirectToActionResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteShouldThrowIfIdIsInvalid()
        {
            var controller = new HotelReviewController(this.context);

            await Assert.ThrowsAsync<NullReferenceException>(() => controller.DeleteConfirmed(-1));
        }
    }
}
