namespace MovieMind.Services.Data
{
    using System.Linq;

    using MovieMind.Data.Models;

    public interface IReviewsService
    {
        IQueryable<Review> GetAll();

        IQueryable<Review> GetPage(int page, int size);

        IQueryable<Review> GetByUserId(string userId);

        IQueryable<Review> GetByMovieId(string movieId);

        int Create(Review review);

        int Edit(Review review);

        void Delete(Review review);

        Review GetById(int id);
    }
}
