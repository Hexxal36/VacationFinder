namespace VacationFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using VacationFinder.Data.Common.Repositories;
    using VacationFinder.Data.Models;

    public class OfferService : IOfferService
    {
        private readonly IDeletableEntityRepository<Offer> offerRepository;

        public OfferService(
            IDeletableEntityRepository<Offer> offerRepository)
        {
            this.offerRepository = offerRepository;
        }

        public IEnumerable<Offer> GetOffers()
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

        private bool IsOfferActive(Offer offer)
        {
            return offer.IsActive;
        }
    }
}
