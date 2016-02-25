namespace MovieMind.Services.Data
{
    using System.Linq;

    using MovieMind.Data.Models;

    public interface IUsersService
    {
        IQueryable<ApplicationUser> GetAll();
    }
}
