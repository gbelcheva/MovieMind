namespace MovieMind.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using MovieMind.Common;

    public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if (!context.Roles.Any())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var role = new IdentityRole { Name = GlobalConstants.AdministratorRoleName };
                roleManager.Create(role);
            }

            const string AdministratorUserName = "admin@admin.com";
            const string AdministratorPassword = "adm1nguru";

            const string User1UserName = "user1@user.com";
            const string User1Password = "user1";

            const string User2UserName = "user2@user.com";
            const string User2Password = "user2";

            if (!context.Users.Any())
            {
                // Create admin user
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var admin = new ApplicationUser { UserName = AdministratorUserName, Email = AdministratorUserName };
                userManager.Create(admin, AdministratorPassword);
                userManager.AddToRole(admin.Id, GlobalConstants.AdministratorRoleName);

                // Create user
                var user1 = new ApplicationUser { UserName = User1UserName, Email = User1UserName };
                userManager.Create(user1, User1Password);

                var user2 = new ApplicationUser { UserName = User2UserName, Email = User2UserName };
                userManager.Create(user2, User2Password);

                var movies = context.Movies.OrderByDescending(m => m.Title).Take(10);
                foreach (var movie in movies)
                {
                    user1.WatchedList.ToList().Add(movie);
                }

                movies = context.Movies.OrderByDescending(m => m.Title).Skip(5).Take(10);
                foreach (var movie in movies)
                {
                    user2.WatchedList.ToList().Add(movie);
                }

                movies = context.Movies.OrderBy(m => m.Genre).Skip(50).Take(20);
                foreach (var movie in movies)
                {
                    user1.WatchList.ToList().Add(movie);
                }

                movies = context.Movies.OrderBy(m => m.Genre).Skip(60).Take(20);
                foreach (var movie in movies)
                {
                    user2.WatchList.ToList().Add(movie);
                }

                context.SaveChanges();
            }
        }
    }
}
