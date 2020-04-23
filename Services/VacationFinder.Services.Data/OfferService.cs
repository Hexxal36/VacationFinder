namespace VacationFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using VacationFinder.Data.Common.Repositories;
    using VacationFinder.Data.Models;

    public class OfferService : IOfferService
    {
        private readonly IDeletableEntityRepository<Offer> offerRepository;
        private readonly IDeletableEntityRepository<Order> orderRepository;

        public OfferService(
            IDeletableEntityRepository<Offer> offerRepository,
            IDeletableEntityRepository<Order> orderRepository)
        {
            this.offerRepository = offerRepository;
            this.orderRepository = orderRepository;
        }

        public IEnumerable<Offer> GetAllOffers()
        {
            List<Offer> query =
                this.offerRepository.All().Where(x => x.IsActive).OrderByDescending(x => x.CreatedOn).ToList();

            return query;
        }

        public IEnumerable<Offer> GetSpecialOffers()
        {
            List<Offer> query =
                this.offerRepository.All().Where(x => x.IsActive && x.IsSpecial).OrderByDescending(x => x.CreatedOn).ToList();

            return query;
        }

        public Offer GetOfferById(int id)
        {
            Offer offer = this.offerRepository.All().Where(x => x.Id == id).ToList().First();

            if (this.IsOfferActive(offer))
            {
                return offer;
            }

            return null;
        }

        public async Task OnOrderAsync(int offerId, int orderId)
        {
            var offer = this.GetOfferById(offerId);

            var order = this.orderRepository.All().Where(x => x.Id == orderId).ToList().First();

            if (offer.IsActive)
            {
                offer.Places -= order.Places;
                await this.offerRepository.SaveChangesAsync();
            }
        }

        public async Task OnOrderDeleteAsync(int offerId, int orderId)
        {
            var offer = this.GetOfferById(offerId);

            var order = this.orderRepository.All().Where(x => x.Id == orderId).ToList().First();

            if (offer.IsActive)
            {
                offer.Places += order.Places;
                await this.offerRepository.SaveChangesAsync();
            }
        }

        private bool IsOfferActive(Offer offer)
        {
            return offer.IsActive;
        }
    }
}
