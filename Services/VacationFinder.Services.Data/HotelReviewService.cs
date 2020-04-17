namespace VacationFinder.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using VacationFinder.Data.Common.Repositories;
    using VacationFinder.Data.Models;

    public class HotelReviewService : IHotelReviewService
    {
        private readonly IDeletableEntityRepository<HotelReview> _hotelReviewRepository;

        public HotelReviewService(
            IDeletableEntityRepository<HotelReview> hotelReviewRepository)
        {
            this._hotelReviewRepository = hotelReviewRepository;
        }

        public async Task CreateAsync(int grade, string title, string body, int hotelId, string userId)
        {
            var hotelReview = new HotelReview
            {
                Grade = grade,
                Title = title,
                Body = body,
                HotelId = hotelId,
                UserId = userId,
            };

            await this._hotelReviewRepository.AddAsync(hotelReview);
            await this._hotelReviewRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var hotelReview = this._hotelReviewRepository.All().Where(x => x.Id == id).ToList().First();

            this._hotelReviewRepository.Delete(hotelReview);

            await this._hotelReviewRepository.SaveChangesAsync();
        }

        public IEnumerable<HotelReview> GetAllReviews()
        {
            return this._hotelReviewRepository.All().ToList();
        }

        public HotelReview GetReviewById(int id)
        {
            HotelReview review = this._hotelReviewRepository.All().Where(x => x.Id == id).ToList().First();

            return review;
        }
    }
}
