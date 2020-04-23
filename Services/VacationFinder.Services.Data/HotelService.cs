namespace VacationFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using VacationFinder.Data.Common.Repositories;
    using VacationFinder.Data.Models;

    public class HotelService : IHotelService
    {
        private readonly IDeletableEntityRepository<Hotel> hotelRepository;

        public HotelService(
            IDeletableEntityRepository<Hotel> hotelRepository)
        {
            this.hotelRepository = hotelRepository;
        }

        public IEnumerable<Hotel> GetAllHotels()
        {
            try
            {
                List<Hotel> query =
                    this.hotelRepository.All().Where(x => x.IsActive).OrderByDescending(x => x.CreatedOn).ToList();

                return query;
            }
            catch
            {
                return null;
            }
        }

        #nullable enable

        public IEnumerable<Hotel> FilterHotels(string? name, int stars, City? city)
        {
            var hotels = this.GetAllHotels();

            if (hotels == null)
            {
                return new List<Hotel>();
            }

            if (name != null)
            {
                hotels = hotels.Where(x => x.Name.Contains(name));
            }

            if (stars != -1)
            {
                hotels = hotels.Where(x => x.Stars == stars);
            }

            if (city != null)
            {
                hotels = hotels.Where(x => x.City.Id == city.Id);
            }

            return hotels.ToList();
        }

        #nullable enable

        public Hotel? GetHotelById(int id)
        {
            Hotel hotel = this.hotelRepository.All().Where(x => x.Id == id).ToList().First();

            if (this.IsHotelActive(hotel))
            {
                return hotel;
            }

            return null;
        }

        private bool IsHotelActive(Hotel hotel)
        {
            return hotel.IsActive;
        }
    }
}
