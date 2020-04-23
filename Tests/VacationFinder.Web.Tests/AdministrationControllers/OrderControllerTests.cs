namespace VacationFinder.Web.Tests.AdministrationControllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using VacationFinder.Common;
    using VacationFinder.Data;
    using VacationFinder.Data.Models;
    using VacationFinder.Data.Models.Enums;
    using VacationFinder.Services.Messaging;
    using VacationFinder.Web.Areas.Administration.Controllers;
    using Xunit;
    using Xunit.Priority;

    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class OrderControllerTests
    {
        private readonly ApplicationDbContext context;
        private readonly IEmailSender emailSender;

        public OrderControllerTests()
        {
            this.context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("Server=.;Database=VacationFinder;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options);
            this.emailSender = new SendGridEmailSender(EmailConstants.SendGridApiKey);

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
                this.context.Add(new Offer()
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

            if (this.context.Orders.Where(x => x.ContactEmail == "TestingOrder@mail.com").Count() == 0)
            {
                this.context.Orders.Add(new Order()
                {
                    ContactEmail = "TestingOrder@mail.com",
                    OfferId = this.context.Offers.First().Id,
                    UserId = this.context.Users.First().Id,
                    Places = 1,
                    IsApproved = false,
                });
            }

            this.context.SaveChanges();
        }

        [Fact]
        [Priority(1)]
        public async Task IndexReturnsView()
        {
            var controller = new OrderController(this.context, this.emailSender);

            var data = await controller.Index();

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task ApproveReturnsApproveViewWithValidData()
        {
            var controller = new OrderController(this.context, this.emailSender);
            var orderId = this.context.Orders.Where(x => x.ContactEmail == "TestingOrder@mail.com").ToList().First().Id;

            var data = await controller.Approve(orderId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task ApproveReturnsNotFoundWithInvalidData()
        {
            var controller = new OrderController(this.context, this.emailSender);

            var data = await controller.Approve(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task ApproveShouldReturnViewResult()
        {
            var controller = new OrderController(this.context, this.emailSender);
            var order = this.context.Orders.Where(x => x.ContactEmail == "TestingOrder@mail.com").ToList().First();

            order.IsApproved = false;
            await this.context.SaveChangesAsync();

            var data = await controller.Approve(order.Id);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task ApproveShouldThrowIfOrderIsAlreadyApproved()
        {
            var controller = new OrderController(this.context, this.emailSender);
            var order = this.context.Orders.Where(x => x.ContactEmail == "TestingOrder@mail.com").ToList().First();

            order.IsApproved = true;
            await this.context.SaveChangesAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(() => controller.Approve(order.Id));
        }

        [Fact]
        [Priority(2)]
        public async Task ApproveShouldReturnNotFoundResultIfIdIsInvalid()
        {
            var controller = new OrderController(this.context, this.emailSender);

            var data = await controller.Approve(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteReturnsDeleteViewWithValidData()
        {
            var controller = new OrderController(this.context, this.emailSender);
            var orderId = this.context.Orders.Where(x => x.ContactEmail == "TestingOrder@mail.com").ToList().First().Id;

            var data = await controller.Delete(orderId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteReturnsDeleteViewWithInvalidData()
        {
            var controller = new OrderController(this.context, this.emailSender);

            var data = await controller.Delete(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteShouldReturnRedirectResult()
        {
            var controller = new OrderController(this.context, this.emailSender);
            var orderId = this.context.Orders.Where(x => x.ContactEmail == "TestingOrder@mail.com").ToList().First().Id;

            var data = await controller.DeleteConfirmed(orderId);

            Assert.IsType<RedirectToActionResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteShouldThrowIfIdIsInvalid()
        {
            var controller = new OrderController(this.context, this.emailSender);

            await Assert.ThrowsAsync<NullReferenceException>(() => controller.DeleteConfirmed(-1));
        }
    }
}
