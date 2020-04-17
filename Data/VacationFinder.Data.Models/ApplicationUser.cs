// ReSharper disable VirtualMemberCallInConstructor
namespace VacationFinder.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;
    using VacationFinder.Data.Common.Models;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.HotelReviews = new HashSet<HotelReview>();
            this.OfferUsers = new HashSet<Order>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<HotelReview> HotelReviews { get; set; }

        public virtual ICollection<Order> OfferUsers { get; set; }
    }
}
