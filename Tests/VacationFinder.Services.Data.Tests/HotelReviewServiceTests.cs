namespace VacationFinder.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using VacationFinder.Data;
    using VacationFinder.Data.Common.Repositories;
    using VacationFinder.Data.Models;
    using VacationFinder.Data.Repositories;
    using Xunit;

    public class HotelReviewServiceTests
    {
        private readonly HotelReviewService service;
        private readonly IDeletableEntityRepository<HotelReview> repository;

        public HotelReviewServiceTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("VFHotelReviewTest");
            var context = new ApplicationDbContext(optionsBuilder.Options);

            var repository =
                new EfDeletableEntityRepository<HotelReview>(context);
            this.repository = repository;
            this.service = new HotelReviewService(repository);
        }

        [Fact]
        public async Task CreateAsyncShouldInsertAnEntryInTheDBCorrectly()
        {
            var count = await this.repository.All().CountAsync();

            await this.service.CreateAsync(4, "Good", "Awesome service.", 0, Guid.NewGuid().ToString());

            Assert.Equal(count + 1, await this.repository.All().CountAsync());

            var reviews = await this.repository.All().ToListAsync();

            Assert.Equal("Good", reviews[count].Title);
        }

        [Fact]
        public async Task DeleteAsyncShouldRemoveEntriesInTheDBCorrectly()
        {
            await this.service.CreateAsync(4, "Good", "Awesome service.", 0, Guid.NewGuid().ToString());
            await this.service.CreateAsync(2, "Meh", "Don't recommend.", 0, Guid.NewGuid().ToString());

            var count = await this.repository.All().CountAsync();

            await this.service.DeleteAsync(1);

            Assert.Equal(count - 1, await this.repository.All().CountAsync());
        }

        [Fact]
        public async Task DeleteAsyncShouldThrowIfNoReviewHasThatId()
        {
            await this.service.CreateAsync(4, "Good", "Awesome service.", 0, Guid.NewGuid().ToString());
            await this.service.CreateAsync(2, "Meh", "Don't recommend.", 0, Guid.NewGuid().ToString());

            var count = await this.repository.All().CountAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(() => this.service.DeleteAsync(count + 100));
            await Assert.ThrowsAsync<InvalidOperationException>(() => this.service.DeleteAsync(-10));
        }

        [Fact]
        public async Task GetAllReviewsShouldReturnEveryReview()
        {
            await this.service.CreateAsync(4, "Good", "Awesome service.", 0, Guid.NewGuid().ToString());
            await this.service.CreateAsync(2, "Meh", "Don't recommend.", 0, Guid.NewGuid().ToString());

            var count = await this.repository.All().CountAsync();

            Assert.Equal(count, this.service.GetAllReviews().ToList().Count());
        }

        [Fact]
        public async Task GetReviewByIdShoudReturnTheCorrectReview()
        {
            await this.service.CreateAsync(4, "Good", "Awesome service.", 0, Guid.NewGuid().ToString());
            await this.service.CreateAsync(2, "Meh", "Don't recommend.", 0, Guid.NewGuid().ToString());

            int count = await this.repository.All().CountAsync();

            var review = this.service.GetReviewById(count - 1);

            Assert.Equal(2, review.Grade);
        }

        [Fact]
        public async Task GetReviewByIdShoudThrowIfThereIsNoReviewWithThatId()
        {
            await this.service.CreateAsync(4, "Good", "Awesome service.", 0, Guid.NewGuid().ToString());
            await this.service.CreateAsync(2, "Meh", "Don't recommend.", 0, Guid.NewGuid().ToString());

            Assert.Throws<InvalidOperationException>(() => this.service.GetReviewById(-1));
        }
    }
}
