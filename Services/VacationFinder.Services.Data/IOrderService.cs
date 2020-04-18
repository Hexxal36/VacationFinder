namespace VacationFinder.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using VacationFinder.Data.Models;

    public interface IOrderService
    {
        Task<Order> CreateAsync(string email, int places, int offerId, string userId);

        Task DeleteAsync(int id);

        IEnumerable<Order> GetAllByUser(string userId);

        Order GetOrderById(int id);
    }
}
