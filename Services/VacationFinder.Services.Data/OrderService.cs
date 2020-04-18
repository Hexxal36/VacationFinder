namespace VacationFinder.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using VacationFinder.Data.Common.Repositories;
    using VacationFinder.Data.Models;

    public class OrderService : IOrderService
    {
        private readonly IDeletableEntityRepository<Order> _orderRepository;

        public OrderService(
            IDeletableEntityRepository<Order> orderRepository)
        {
            this._orderRepository = orderRepository;
        }

        public async Task<Order> CreateAsync(string email, int places, int offerId, string userId)
        {
            var order = new Order
            {
                IsApproved = false,
                ContactEmail = email,
                Places = places,
                OfferId = offerId,
                UserId = userId,
            };

            await this._orderRepository.AddAsync(order);
            await this._orderRepository.SaveChangesAsync();

            return order;
        }

        public async Task DeleteAsync(int id)
        {
            var order = this._orderRepository.All().Where(x => x.Id == id).ToList().First();

            this._orderRepository.Delete(order);

            await this._orderRepository.SaveChangesAsync();
        }

        public IEnumerable<Order> GetAllByUser(string userId)
        {
            return this._orderRepository.All().Where(x => x.UserId == userId).ToList();
        }

        public Order GetOrderById(int id)
        {
            Order order = this._orderRepository.All().Where(x => x.Id == id).ToList().First();

            return order;
        }
    }
}
