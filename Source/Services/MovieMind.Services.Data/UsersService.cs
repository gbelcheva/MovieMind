namespace MovieMind.Services.Data
{
    using System.Linq;

    using MovieMind.Data.Common;
    using MovieMind.Data.Models;

    public class UsersService : IUsersService
    {
        private readonly IGenericDbRepository<ApplicationUser> users;

        public UsersService(IGenericDbRepository<ApplicationUser> usersRepo)
        {
            this.users = usersRepo;
        }

        public IQueryable<ApplicationUser> GetAll()
        {
            return this.users.All();
        }
    }
}
