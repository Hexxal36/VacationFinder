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

    public class OfferServiceTests
    {
        private readonly OfferService service;
        private readonly IDeletableEntityRepository<Offer> offerRepository;
        private readonly IDeletableEntityRepository<Order> orderRepository;

        public OfferServiceTests()
        {
            DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder =
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("VFOfferTest");

            ApplicationDbContext context =
            new ApplicationDbContext(optionsBuilder.Options);

            this.offerRepository = new EfDeletableEntityRepository<Offer>(context);
            this.orderRepository = new EfDeletableEntityRepository<Order>(context);
            this.service = new OfferService(this.offerRepository, this.orderRepository);
        }

        [Fact]
        public async Task GetAllOffersShouldReturnAllOffers()
        {
            var count = this.service.GetAllOffers().Count();

            await this.offerRepository.AddAsync(new Offer() { IsActive = true });
            await this.offerRepository.AddAsync(new Offer());
            await this.offerRepository.SaveChangesAsync();

            Assert.Equal(count + 1, this.service.GetAllOffers().Count());
        }

        [Fact]
        public async Task GetAllSpecialOffersShouldReturnOnlySpecialOffers()
        {
            var count = this.service.GetAllOffers().Count();

            await this.offerRepository.AddAsync(new Offer() { IsActive = true, IsSpecial = true });
            await this.offerRepository.AddAsync(new Offer() { IsActive = true });
            await this.offerRepository.AddAsync(new Offer() { IsActive = true, IsSpecial = true });
            await this.offerRepository.AddAsync(new Offer() { IsActive = true });
            await this.offerRepository.SaveChangesAsync();

            Assert.Equal(count + 4, this.service.GetAllOffers().Count());
        }

        [Fact]
        public async Task GetOfferByIdReturnsCorrectOffer()
        {
            var count = await this.offerRepository.All().CountAsync();

            await this.offerRepository.AddAsync(new Offer() { IsActive = true, Id = count + 1 });
            await this.offerRepository.AddAsync(new Offer() { IsActive = true, Id = count + 2 });
            await this.offerRepository.AddAsync(new Offer() { IsActive = true, Id = count + 3 });
            await this.offerRepository.AddAsync(new Offer() { IsActive = true, Id = count + 4 });
            await this.offerRepository.SaveChangesAsync();

            var offer = this.service.GetOfferById(count + 1);

            Assert.Equal(count + 1, offer.Id);
        }

        [Fact]
        public async Task GetOfferByIdShouldReturnNullIfTheOfferIsInactive()
        {
            var count = await this.offerRepository.All().CountAsync();

            await this.offerRepository.AddAsync(new Offer() { IsActive = true, Id = count + 1 });
            await this.offerRepository.AddAsync(new Offer() { IsActive = false, Id = count + 2 });
            await this.offerRepository.AddAsync(new Offer() { IsActive = true, Id = count + 3 });
            await this.offerRepository.AddAsync(new Offer() { IsActive = true, Id = count + 4 });
            await this.offerRepository.SaveChangesAsync();

            var offer = this.service.GetOfferById(count + 2);

            Assert.Null(offer);
        }

        [Fact]
        public async Task GetOfferByIdShouldThrowIfOfferDoesNotExist()
        {
            var count = await this.offerRepository.All().CountAsync();

            await this.offerRepository.AddAsync(new Offer() { IsActive = true, Id = count + 1 });
            await this.offerRepository.AddAsync(new Offer() { IsActive = false, Id = count + 2 });
            await this.offerRepository.AddAsync(new Offer() { IsActive = true, Id = count + 3 });
            await this.offerRepository.AddAsync(new Offer() { IsActive = true, Id = count + 4 });
            await this.offerRepository.SaveChangesAsync();

            Assert.Throws<InvalidOperationException>(() => this.service.GetOfferById(-100));
            Assert.Throws<InvalidOperationException>(() => this.service.GetOfferById(0));
        }

        [Fact]
        public async Task OnOfferShouldUpdateTheOfferPlacesCorrectly()
        {
            var count = await this.offerRepository.All().CountAsync();

            await this.offerRepository.AddAsync(new Offer() { IsActive = true, Id = count + 1, Places = 10 });
            await this.offerRepository.SaveChangesAsync();

            await this.orderRepository.AddAsync(new Order() { Id = count + 1, OfferId = count + 1, Places = 2 });
            await this.orderRepository.SaveChangesAsync();

            await this.service.OnOrderAsync(count + 1, count + 1);

            Assert.Equal(8, this.service.GetOfferById(count + 1).Places);
        }

        [Fact]
        public async Task OnOfferDeleteShouldUpdateTheOfferPlacesCorrectly()
        {
            var count = await this.offerRepository.All().CountAsync();

            await this.offerRepository.AddAsync(new Offer() { IsActive = true, Id = count + 1, Places = 10 });
            await this.offerRepository.SaveChangesAsync();

            await this.orderRepository.AddAsync(new Order() { Id = count + 1, OfferId = count + 1, Places = 2 });
            await this.orderRepository.SaveChangesAsync();

            await this.service.OnOrderDeleteAsync(count + 1, count + 1);

            Assert.Equal(12, this.service.GetOfferById(count + 1).Places);
        }
    }
}
