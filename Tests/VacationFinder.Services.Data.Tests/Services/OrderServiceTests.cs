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

    public class OrderServiceTests
    {
        [Fact]
        public async Task CreateAsyncShouldCreateNewEntriesInTheDB()
        {
            var repository = this.CreateRepository();
            var service = new OrderService(repository);

            var count = await repository.All().CountAsync();

            await service.CreateAsync("test@test.com", 1, 1, Guid.NewGuid().ToString());

            Assert.Equal(count + 1, await repository.All().CountAsync());
        }

        [Fact]
        public async Task DeleteAsyncShouldDeleteEtriesCorrectly()
        {
            var repository = this.CreateRepository();
            var service = new OrderService(repository);

            await service.CreateAsync("test@test.com", 1, 1, Guid.NewGuid().ToString());

            var count = await repository.All().CountAsync();

            await service.DeleteAsync(1);

            Assert.Equal(count - 1, await repository.All().CountAsync());
        }

        [Fact]
        public async Task GetAllByUserShouldReturnOnlyOrdersFromOneUser()
        {
            var repository = this.CreateRepository();
            var service = new OrderService(repository);

            var userGuid = Guid.NewGuid().ToString();

            await service.CreateAsync("test@test.com", 1, 1, userGuid);
            await service.CreateAsync("test@test.com", 1, 1, Guid.NewGuid().ToString());
            await service.CreateAsync("test@test.com", 1, 1, userGuid);
            await service.CreateAsync("test@test.com", 1, 1, Guid.NewGuid().ToString());

            Assert.Equal(2, service.GetAllByUser(userGuid).ToList().Count());

            foreach (var order in service.GetAllByUser(userGuid))
            {
                Assert.Equal(userGuid, order.UserId);
            }
        }

        [Fact]
        public async Task GetOrderByIdShoudReturnTheCorrectReview()
        {
            var repository = this.CreateRepository();

            var service = new OrderService(repository);

            await service.CreateAsync("test@test.com", 1, 1, Guid.NewGuid().ToString());
            await service.CreateAsync("test@test.com", 1, 1, Guid.NewGuid().ToString());
            await service.CreateAsync("test@test.com", 1, 1, Guid.NewGuid().ToString());

            var count = await repository.All().CountAsync();

            var order = service.GetOrderById(count - 1);

            Assert.Equal(count - 1, order.Id);
        }

        [Fact]
        public void GetOrderByIdShoudThrowIfThereIsNoReviewWithThatId()
        {
            var repository = this.CreateRepository();

            var service = new OrderService(repository);

            Assert.Throws<InvalidOperationException>(() => service.GetOrderById(-1));
            Assert.Throws<InvalidOperationException>(() => service.GetOrderById(0));
        }

        private IDeletableEntityRepository<Order> CreateRepository()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("VFOrderTest");
            var context = new ApplicationDbContext(optionsBuilder.Options);

            var repository =
                new EfDeletableEntityRepository<Order>(context);
            return repository;
        }
    }
}
