namespace MovieMind.Services.Data
{
    using System.Linq;

    using MovieMind.Data.Models;

    public interface IMoviesService
    {
        IQueryable<Movie> GetAll();

        IQueryable<Movie> PagedGetAll(int page, int size);

        IQueryable<Movie> GetTrending();

        IQueryable<Movie> GetRecommended(string userId);

        int Create(Movie movie);

        int Edit(Movie movie);

        void Delete(Movie movie);

        Movie GetById(string id);

        IQueryable<Movie> Search(string query, string country, string language, string genre);
    }
}
