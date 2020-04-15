namespace VacationFinder.Services.Data
{
    using System.Collections.Generic;

    using VacationFinder.Data.Models;

    public interface IOfferService
    {
        IEnumerable<Offer> GetSpecialOffers();

        IEnumerable<Offer> GetOffers();

        Offer GetOfferById(int id);
    }
}
