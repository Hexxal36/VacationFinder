namespace VacationFinder.Web.ViewModels.Administration.User
{
    using System.ComponentModel.DataAnnotations;

    public class UserViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
