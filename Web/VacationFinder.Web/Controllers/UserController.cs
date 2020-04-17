namespace VacationFinder.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using VacationFinder.Services.Data;

    public class UserController : Controller
    {
        private readonly IOrderService orderService;

        public UserController(
             IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public IActionResult ShowOrders()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = this.orderService.GetAllByUser(userId);

            return this.View(orders);
        }
    }
}