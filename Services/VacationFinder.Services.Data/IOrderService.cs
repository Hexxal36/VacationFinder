namespace VacationFinder.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using VacationFinder.Data.Models;

    public interface IOrderService
    {
        Task CreateAsync(string email, int offerId, string userId);

        Task DeleteAsync(int id);

        IEnumerable<Order> GetAllByUser(string userId);
    }
}
