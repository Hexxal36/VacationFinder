namespace VacationFinder.Web.Tests.AdministrationControllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using VacationFinder.Data;
    using VacationFinder.Data.Models;
    using VacationFinder.Web.Areas.Administration.Controllers;
    using Xunit;
    using Xunit.Priority;

    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class TransportControllerTests
    {
        private readonly ApplicationDbContext context;

        public TransportControllerTests()
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
            var controller = new TransportController(this.context);

            var data = await controller.Index();

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(1)]
        public void CreateReturnsView()
        {
            var controller = new TransportController(this.context);
            Assert.IsType<ViewResult>(controller.Create());
        }

        [Fact]
        [Priority(1)]
        public async Task CreateReturnsRedirectResultAndCreatesAnEntityInTheDB()
        {
            var controller = new TransportController(this.context);
            var currentTransports = this.context.Transports.ToList().Count;

            var transport = new Transport()
            {
                Sort = 1,
                Title = "TestingTransport",
                IsActive = true,
            };

            var createData = await controller.Create(transport);

            Assert.IsType<RedirectToActionResult>(createData);
            Assert.Equal(currentTransports + 1, this.context.Transports.ToList().Count);
        }

        [Fact]
        [Priority(1)]
        public async Task CreateThrowsExceptionWithInvalidData()
        {
            var controller = new TransportController(this.context);
            var currentTransports = this.context.Transports.ToList().Count;

            var transport = new Transport()
            {
                Sort = 1,
                Title = null,
                IsActive = true,
            };

            await Assert.ThrowsAsync<DbUpdateException>(() => controller.Create(transport));
            Assert.Equal(currentTransports, this.context.Transports.ToList().Count);
        }

        [Fact]
        [Priority(2)]
        public async Task DetailsReturnViewResultWithExistingTransport()
        {
            var controller = new TransportController(this.context);
            var transportId = this.context.Transports.Where(x => x.Title == "TestingTransport").ToList().First().Id;

            var data = await controller.Details(transportId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task DetailsReturnNotFoundWithInvalidData()
        {
            var controller = new TransportController(this.context);

            var data = await controller.Details(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task EditReturnsViewWithExistingTransport()
        {
            var controller = new TransportController(this.context);
            var transportId = this.context.Transports.Where(x => x.Title == "TestingTransport").ToList().First().Id;

            var data = await controller.Edit(transportId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task EditReturnsNotFoundWithInvalidData()
        {
            var controller = new TransportController(this.context);

            var data = await controller.Edit(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(3)]
        public async Task EditReturnViewResultWithExistingEntry()
        {
            var controller = new TransportController(this.context);
            var transportId = this.context.Transports.Where(x => x.Title == "TestingTransport").ToList().First().Id;

            var transport = this.context.Transports.Where(x => x.Id == transportId).ToList().First();

            transport.Sort = 2;

            var data = await controller.Edit(transportId, transport);

            Assert.IsType<RedirectToActionResult>(data);

            var editedTransport = this.context.Transports.Where(x => x.Id == transportId).ToList().First();

            Assert.Equal(2, editedTransport.Sort);
        }

        [Fact]
        [Priority(3)]
        public async Task EditReturnNotFoundResultWithInvalidData()
        {
            var controller = new TransportController(this.context);

            var data = await controller.Edit(-1, new Transport());

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteReturnsDeleteViewWithValidData()
        {
            var controller = new TransportController(this.context);
            var transportId = this.context.Transports.Where(x => x.Title == "TestingTransport").ToList().First().Id;

            var data = await controller.Delete(transportId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteReturnsDeleteViewWithInvalidData()
        {
            var controller = new TransportController(this.context);

            var data = await controller.Delete(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteShouldReturnRedirectResult()
        {
            var controller = new TransportController(this.context);
            var transportId = this.context.Transports.Where(x => x.Title == "TestingTransport").ToList().First().Id;

            var data = await controller.DeleteConfirmed(transportId);

            Assert.IsType<RedirectToActionResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteShouldThrowIfIdIsInvalid()
        {
            var controller = new TransportController(this.context);

            await Assert.ThrowsAsync<NullReferenceException>(() => controller.DeleteConfirmed(-1));
        }
    }
}
