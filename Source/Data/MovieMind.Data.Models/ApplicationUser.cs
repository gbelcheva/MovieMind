namespace MovieMind.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
            : base()
        {
            this.WatchList = new HashSet<Movie>();
            this.WatchedList = new HashSet<Movie>();
            this.Reviews = new HashSet<Review>();
        }

        public int Age { get; set; }

        public Genders Gender { get; set; }

        public UserCountries UserCountry { get; set; }

        public string Avatar { get; set; }

        public virtual IEnumerable<Movie> WatchList { get; set; }

        public virtual IEnumerable<Movie> WatchedList { get; set; }

        public virtual IEnumerable<Review> Reviews { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            return userIdentity;
        }
    }
}
