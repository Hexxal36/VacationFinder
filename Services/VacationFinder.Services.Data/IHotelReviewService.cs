namespace VacationFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using VacationFinder.Data.Models;

    public interface IHotelReviewService
    {
        Task CreateAsync(int grade, string title, string body, int hotelId, string userId);

        Task DeleteAsync(int id);

        IEnumerable<HotelReview> GetAllReviews();

        HotelReview GetReviewById(int id);
    }
}
