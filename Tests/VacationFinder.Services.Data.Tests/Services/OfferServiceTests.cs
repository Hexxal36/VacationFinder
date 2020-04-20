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

    public class OfferServiceTests
    {
        private static DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder =
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("VFOfferTest");

        private ApplicationDbContext context =
            new ApplicationDbContext(optionsBuilder.Options);

        [Fact]
        public async Task GetAllOffersShouldReturnAllOffers()
        {
            var offerRepo = this.CreateOfferRepository();
            var orderRepo = this.CreateOrderRepository();
            var service = new OfferService(offerRepo, orderRepo);
            var count = service.GetAllOffers().Count();

            await offerRepo.AddAsync(new Offer() { IsActive = true });
            await offerRepo.AddAsync(new Offer());
            await offerRepo.SaveChangesAsync();

            Assert.Equal(count + 2, await offerRepo.All().CountAsync());
            Assert.Equal(count + 1, service.GetAllOffers().Count());
        }

        [Fact]
        public async Task GetAllSpecialOffersShouldReturnOnlySpecialOffers()
        {
            var offerRepo = this.CreateOfferRepository();
            var orderRepo = this.CreateOrderRepository();
            var service = new OfferService(offerRepo, orderRepo);

            var count = service.GetAllOffers().Count();

            await offerRepo.AddAsync(new Offer() { IsActive = true, IsSpecial = true });
            await offerRepo.AddAsync(new Offer() { IsActive = true });
            await offerRepo.AddAsync(new Offer() { IsActive = true, IsSpecial = true });
            await offerRepo.AddAsync(new Offer() { IsActive = true });
            await offerRepo.SaveChangesAsync();

            Assert.Equal(count + 4, service.GetAllOffers().Count());
        }

        [Fact]
        public async Task GetOfferByIdReturnsCorrectOffer()
        {
            var offerRepo = this.CreateOfferRepository();
            var orderRepo = this.CreateOrderRepository();
            var service = new OfferService(offerRepo, orderRepo);

            var count = await offerRepo.All().CountAsync();

            await offerRepo.AddAsync(new Offer() { IsActive = true, Id = count + 1 });
            await offerRepo.AddAsync(new Offer() { IsActive = true, Id = count + 2 });
            await offerRepo.AddAsync(new Offer() { IsActive = true, Id = count + 3 });
            await offerRepo.AddAsync(new Offer() { IsActive = true, Id = count + 4 });
            await offerRepo.SaveChangesAsync();

            var offer = service.GetOfferById(count + 1);

            Assert.Equal(count + 1, offer.Id);
        }

        [Fact]
        public async Task GetOfferByIdShouldReturnNullIfTheOfferIsInactive()
        {
            var offerRepo = this.CreateOfferRepository();
            var orderRepo = this.CreateOrderRepository();
            var service = new OfferService(offerRepo, orderRepo);

            var count = await offerRepo.All().CountAsync();

            await offerRepo.AddAsync(new Offer() { IsActive = true, Id = count + 1 });
            await offerRepo.AddAsync(new Offer() { IsActive = false, Id = count + 2 });
            await offerRepo.AddAsync(new Offer() { IsActive = true, Id = count + 3 });
            await offerRepo.AddAsync(new Offer() { IsActive = true, Id = count + 4 });
            await offerRepo.SaveChangesAsync();

            var offer = service.GetOfferById(count + 2);

            Assert.Null(offer);
        }

        [Fact]
        public async Task GetOfferByIdShouldThrowIfOfferDoesNotExist()
        {
            var offerRepo = this.CreateOfferRepository();
            var orderRepo = this.CreateOrderRepository();
            var service = new OfferService(offerRepo, orderRepo);
            var count = await offerRepo.All().CountAsync();

            await offerRepo.AddAsync(new Offer() { IsActive = true, Id = count + 1 });
            await offerRepo.AddAsync(new Offer() { IsActive = false, Id = count + 2 });
            await offerRepo.AddAsync(new Offer() { IsActive = true, Id = count + 3 });
            await offerRepo.AddAsync(new Offer() { IsActive = true, Id = count + 4 });
            await offerRepo.SaveChangesAsync();

            Assert.Throws<InvalidOperationException>(() => service.GetOfferById(-100));
            Assert.Throws<InvalidOperationException>(() => service.GetOfferById(0));
        }

        [Fact]
        public async Task OnOfferShouldUpdateTheOfferPlacesCorrectly()
        {
            var offerRepo = this.CreateOfferRepository();
            var orderRepo = this.CreateOrderRepository();
            var service = new OfferService(offerRepo, orderRepo);
            var count = await offerRepo.All().CountAsync();

            await offerRepo.AddAsync(new Offer() { IsActive = true, Id = count + 1, Places = 10 });
            await offerRepo.SaveChangesAsync();

            await orderRepo.AddAsync(new Order() { Id = count + 1, OfferId = count + 1, Places = 2 });
            await orderRepo.SaveChangesAsync();

            await service.OnOrderAsync(count + 1, count + 1);

            Assert.Equal(8, service.GetOfferById(count + 1).Places);
        }

        [Fact]
        public async Task OnOfferDeleteShouldUpdateTheOfferPlacesCorrectly()
        {
            var offerRepo = this.CreateOfferRepository();
            var orderRepo = this.CreateOrderRepository();
            var service = new OfferService(offerRepo, orderRepo);
            var count = await offerRepo.All().CountAsync();

            await offerRepo.AddAsync(new Offer() { IsActive = true, Id = count + 1, Places = 10 });
            await offerRepo.SaveChangesAsync();

            await orderRepo.AddAsync(new Order() { Id = count + 1, OfferId = count + 1, Places = 2 });
            await orderRepo.SaveChangesAsync();

            await service.OnOrderDeleteAsync(count + 1, count + 1);

            Assert.Equal(12, service.GetOfferById(count + 1).Places);
        }

        private IDeletableEntityRepository<Offer> CreateOfferRepository()
        {
            return new EfDeletableEntityRepository<Offer>(this.context);
        }

        private IDeletableEntityRepository<Order> CreateOrderRepository()
        {
            return new EfDeletableEntityRepository<Order>(this.context);
        }
    }
}
