namespace VacationFinder.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using VacationFinder.Data.Models;

    public interface IHotelService
    {
        IEnumerable<Hotel> GetAllHotels();

        #nullable enable

        IEnumerable<Hotel> FilterHotels(IEnumerable<Hotel> hotels, string? name, int stars, City? city);

        Hotel? GetHotelById(int id);
    }
}
