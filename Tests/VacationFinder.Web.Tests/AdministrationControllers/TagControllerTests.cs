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
    public class TagControllerTests
    {
        private readonly ApplicationDbContext context;

        public TagControllerTests()
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
            var controller = new TagController(this.context);

            var data = await controller.Index();

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(1)]
        public void CreateReturnsView()
        {
            var controller = new TagController(this.context);
            Assert.IsType<ViewResult>(controller.Create());
        }

        [Fact]
        [Priority(1)]
        public async Task CreateReturnsRedirectResultAndCreatesAnEntityInTheDB()
        {
            var controller = new TagController(this.context);
            var currentTags = this.context.Tags.ToList().Count;

            var tag = new Tag()
            {
                Sort = 1,
                Title = "TestingTag",
                IsActive = true,
            };

            var createData = await controller.Create(tag);

            Assert.IsType<RedirectToActionResult>(createData);
            Assert.Equal(currentTags + 1, this.context.Tags.ToList().Count);
        }

        [Fact]
        [Priority(1)]
        public async Task CreateThrowsExceptionWithInvalidData()
        {
            var controller = new TagController(this.context);
            var currentTags = this.context.Tags.ToList().Count;

            var tag = new Tag()
            {
                Sort = 1,
                Title = null,
                IsActive = true,
            };

            await Assert.ThrowsAsync<DbUpdateException>(() => controller.Create(tag));
            Assert.Equal(currentTags, this.context.Tags.ToList().Count);
        }

        [Fact]
        [Priority(2)]
        public async Task DetailsReturnViewResultWithExistingTag()
        {
            var controller = new TagController(this.context);
            var tagId = this.context.Tags.Where(x => x.Title == "TestingTag").ToList().First().Id;

            var data = await controller.Details(tagId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task DetailsReturnNotFoundWithInvalidData()
        {
            var controller = new TagController(this.context);

            var data = await controller.Details(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task EditReturnsViewWithExistingTag()
        {
            var controller = new TagController(this.context);
            var tagId = this.context.Tags.Where(x => x.Title == "TestingTag").ToList().First().Id;

            var data = await controller.Edit(tagId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(2)]
        public async Task EditReturnsNotFoundWithInvalidData()
        {
            var controller = new TagController(this.context);

            var data = await controller.Edit(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(3)]
        public async Task EditReturnViewResultWithExistingEntry()
        {
            var controller = new TagController(this.context);
            var tagId = this.context.Tags.Where(x => x.Title == "TestingTag").ToList().First().Id;

            var tag = this.context.Tags.Where(x => x.Id == tagId).ToList().First();

            tag.Sort = 2;

            var data = await controller.Edit(tagId, tag);

            Assert.IsType<RedirectToActionResult>(data);

            var editedTag = this.context.Tags.Where(x => x.Id == tagId).ToList().First();

            Assert.Equal(2, editedTag.Sort);
        }

        [Fact]
        [Priority(3)]
        public async Task EditReturnNotFoundResultWithInvalidData()
        {
            var controller = new TagController(this.context);

            var data = await controller.Edit(-1, new Tag());

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteReturnsDeleteViewWithValidData()
        {
            var controller = new TagController(this.context);
            var tagId = this.context.Tags.Where(x => x.Title == "TestingTag").ToList().First().Id;

            var data = await controller.Delete(tagId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteReturnsDeleteViewWithInvalidData()
        {
            var controller = new TagController(this.context);

            var data = await controller.Delete(-1);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteShouldReturnRedirectResult()
        {
            var controller = new TagController(this.context);
            var tagId = this.context.Tags.Where(x => x.Title == "TestingTag").ToList().First().Id;

            var data = await controller.DeleteConfirmed(tagId);

            Assert.IsType<RedirectToActionResult>(data);
        }

        [Fact]
        [Priority(4)]
        public async Task DeleteShouldThrowIfIdIsInvalid()
        {
            var controller = new TagController(this.context);

            await Assert.ThrowsAsync<NullReferenceException>(() => controller.DeleteConfirmed(-1));
        }
    }
}
