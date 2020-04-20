namespace VacationFinder.Services.Data.Tests.Services
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
        [Fact]
        public async Task CreateAsyncShouldInsertAnEntryInTheDBCorrectly()
        {
            var repository = this.CreateRepository();
            var service = new HotelReviewService(repository);

            var count = await repository.All().CountAsync();

            await service.CreateAsync(4, "Good", "Awesome service.", 0, Guid.NewGuid().ToString());

            Assert.Equal(count + 1, await repository.All().CountAsync());

            var reviews = await repository.All().ToListAsync();

            Assert.Equal("Good", reviews[count].Title);
        }

        [Fact]
        public async Task DeleteAsyncShouldRemoveEntriesInTheDBCorrectly()
        {
            var repository = this.CreateRepository();
            var service = new HotelReviewService(repository);

            await service.CreateAsync(4, "Good", "Awesome service.", 0, Guid.NewGuid().ToString());
            await service.CreateAsync(2, "Meh", "Don't recommend.", 0, Guid.NewGuid().ToString());

            var count = await repository.All().CountAsync();

            await service.DeleteAsync(1);

            Assert.Equal(count - 1, await repository.All().CountAsync());
        }

        [Fact]
        public async Task DeleteAsyncShouldThrowIfNoReviewHasThatId()
        {
            var repository = this.CreateRepository();
            var service = new HotelReviewService(repository);

            await service.CreateAsync(4, "Good", "Awesome service.", 0, Guid.NewGuid().ToString());
            await service.CreateAsync(2, "Meh", "Don't recommend.", 0, Guid.NewGuid().ToString());

            var count = await repository.All().CountAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteAsync(count + 100));
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteAsync(-10));
        }

        [Fact]
        public async Task GetAllReviewsShouldReturnEveryReview()
        {
            var repository = this.CreateRepository();

            var service = new HotelReviewService(repository);

            await service.CreateAsync(4, "Good", "Awesome service.", 0, Guid.NewGuid().ToString());
            await service.CreateAsync(2, "Meh", "Don't recommend.", 0, Guid.NewGuid().ToString());

            var count = await repository.All().CountAsync();

            Assert.Equal(count, service.GetAllReviews().ToList().Count());
        }

        [Fact]
        public async Task GetReviewByIdShoudReturnTheCorrectReview()
        {
            var repository = this.CreateRepository();

            var service = new HotelReviewService(repository);

            await service.CreateAsync(4, "Good", "Awesome service.", 0, Guid.NewGuid().ToString());
            await service.CreateAsync(2, "Meh", "Don't recommend.", 0, Guid.NewGuid().ToString());

            var count = await repository.All().CountAsync();

            var review = service.GetReviewById(count - 1);

            Assert.Equal(2, review.Grade);
        }

        [Fact]
        public async Task GetReviewByIdShoudThrowIfThereIsNoReviewWithThatId()
        {
            var repository = this.CreateRepository();

            var service = new HotelReviewService(repository);

            await service.CreateAsync(4, "Good", "Awesome service.", 0, Guid.NewGuid().ToString());
            await service.CreateAsync(2, "Meh", "Don't recommend.", 0, Guid.NewGuid().ToString());

            Assert.Throws<InvalidOperationException>(() => service.GetReviewById(1));
        }

        private IDeletableEntityRepository<HotelReview> CreateRepository()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("VFHotelReviewTest");
            var context = new ApplicationDbContext(optionsBuilder.Options);

            var repository =
                new EfDeletableEntityRepository<HotelReview>(context);
            return repository;
        }
    }
}
