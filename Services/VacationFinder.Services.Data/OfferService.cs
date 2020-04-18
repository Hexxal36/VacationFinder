namespace VacationFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using VacationFinder.Data.Common.Repositories;
    using VacationFinder.Data.Models;

    public class OfferService : IOfferService
    {
        private readonly IDeletableEntityRepository<Offer> _offerRepository;
        private readonly IDeletableEntityRepository<Order> _orderRepository;

        public OfferService(
            IDeletableEntityRepository<Offer> offerRepository,
            IDeletableEntityRepository<Order> orderRepository)
        {
            this._offerRepository = offerRepository;
            this._orderRepository = orderRepository;
        }

        public IEnumerable<Offer> GetOffers()
        {
            List<Offer> query =
                this._offerRepository.All().Where(x => x.IsActive).OrderByDescending(x => x.CreatedOn).ToList();

            return query;
        }

        public IEnumerable<Offer> GetSpecialOffers()
        {
            List<Offer> query =
                this._offerRepository.All().Where(x => x.IsActive && x.IsSpecial).OrderByDescending(x => x.CreatedOn).ToList();

            return query;
        }

        public Offer GetOfferById(int id)
        {
            Offer offer = this._offerRepository.All().Where(x => x.Id == id).ToList().First();

            if (this.IsOfferActive(offer))
            {
                return offer;
            }

            return null;
        }

        public async Task OnOrder(int offerId, int orderId)
        {
            var offer = this.GetOfferById(offerId);

            var order = this._orderRepository.All().Where(x => x.Id == orderId).ToList().First();

            offer.Places -= order.Places;

            await this._offerRepository.SaveChangesAsync();
        }

        public async Task OnOrderDelete(int offerId, int orderId)
        {
            var offer = this.GetOfferById(offerId);

            var order = this._orderRepository.All().Where(x => x.Id == orderId).ToList().First();

            offer.Places += order.Places;

            await this._offerRepository.SaveChangesAsync();
        }

        private bool IsOfferActive(Offer offer)
        {
            return offer.IsActive;
        }
    }
}
