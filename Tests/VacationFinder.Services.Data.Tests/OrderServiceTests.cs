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

    public class OrderServiceTests
    {
        private readonly OrderService service;
        private readonly IDeletableEntityRepository<Order> repository;

        public OrderServiceTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("VFOrderTest");
            var context = new ApplicationDbContext(optionsBuilder.Options);

            var repository =
                new EfDeletableEntityRepository<Order>(context);

            this.repository = repository;
            this.service = new OrderService(repository);
        }

        [Fact]
        public async Task CreateAsyncShouldCreateNewEntriesInTheDB()
        {
            var count = await this.repository.All().CountAsync();

            await this.service.CreateAsync("test@test.com", 1, 1, Guid.NewGuid().ToString());

            Assert.Equal(count + 1, await this.repository.All().CountAsync());
        }

        [Fact]
        public async Task DeleteAsyncShouldDeleteEtriesCorrectly()
        {
            await this.service.CreateAsync("test@test.com", 1, 1, Guid.NewGuid().ToString());

            var count = await this.repository.All().CountAsync();

            await this.service.DeleteAsync(1);

            Assert.Equal(count - 1, await this.repository.All().CountAsync());
        }

        [Fact]
        public async Task GetAllByUserShouldReturnOnlyOrdersFromOneUser()
        {
            var userGuid = Guid.NewGuid().ToString();

            await this.service.CreateAsync("test@test.com", 1, 1, userGuid);
            await this.service.CreateAsync("test@test.com", 1, 1, Guid.NewGuid().ToString());
            await this.service.CreateAsync("test@test.com", 1, 1, userGuid);
            await this.service.CreateAsync("test@test.com", 1, 1, Guid.NewGuid().ToString());

            Assert.Equal(2, this.service.GetAllByUser(userGuid).ToList().Count());

            foreach (var order in this.service.GetAllByUser(userGuid))
            {
                Assert.Equal(userGuid, order.UserId);
            }
        }

        [Fact]
        public async Task GetOrderByIdShoudReturnTheCorrectReview()
        {
            await this.service.CreateAsync("test@test.com", 1, 1, Guid.NewGuid().ToString());
            await this.service.CreateAsync("test@test.com", 1, 1, Guid.NewGuid().ToString());
            await this.service.CreateAsync("test@test.com", 1, 1, Guid.NewGuid().ToString());

            var count = await this.repository.All().CountAsync();

            var order = this.service.GetOrderById(count - 1);

            Assert.Equal(count - 1, order.Id);
        }

        [Fact]
        public void GetOrderByIdShoudThrowIfThereIsNoReviewWithThatId()
        {
            Assert.Throws<InvalidOperationException>(() => this.service.GetOrderById(-1));
            Assert.Throws<InvalidOperationException>(() => this.service.GetOrderById(0));
        }
    }
}
