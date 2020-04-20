namespace VacationFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VacationFinder.Data.Models;

    public interface IOfferService
    {
        IEnumerable<Offer> GetSpecialOffers();

        IEnumerable<Offer> GetAllOffers();

        Task OnOrderAsync(int offerId, int orderId);

        Task OnOrderDeleteAsync(int offerId, int orderId);

        Offer GetOfferById(int id);
    }
}
