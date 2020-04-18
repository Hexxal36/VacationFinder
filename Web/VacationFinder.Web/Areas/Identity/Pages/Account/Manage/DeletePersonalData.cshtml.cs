namespace VacationFinder.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using VacationFinder.Common;
    using VacationFinder.Data;
    using VacationFinder.Data.Models;
    using VacationFinder.Services.Data;

    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;
        private readonly IHotelReviewService _hotelReviewService;
        private readonly IOrderService _orderService;
        private readonly ApplicationDbContext _context;

        public DeletePersonalDataModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            IHotelReviewService hotelReviewService,
            IOrderService orderService,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _hotelReviewService = hotelReviewService;
            _orderService = orderService;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }

            foreach (var review in user.HotelReviews)
            {
                await this._hotelReviewService.DeleteAsync(review.Id);
            }

            foreach (var order in user.Orders)
            {
                await this._orderService.DeleteAsync(order.Id);
            }

            if (await _userManager.IsInRoleAsync(user, GlobalConstants.SuperAdministratorRoleName))
            {
                user.IsDeleted = true;
                user.DeletedOn = DateTime.UtcNow;
                await this._context.SaveChangesAsync();
                var userId = await _userManager.GetUserIdAsync(user);

                await _signInManager.SignOutAsync();

                _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);
            }


            return Redirect("~/");
        }
    }
}
