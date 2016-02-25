namespace MovieMind.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using MovieMind.Data.Common;
    using MovieMind.Data.Models;
    using Web;

    public class MoviesService : IMoviesService
    {
        private readonly IDbRepository<Movie> movies;
        private readonly IDbRepository<Review> reviews;
        private readonly IGenericDbRepository<ApplicationUser> users;
        private readonly IIdentifierProvider identifierProvider;

        public MoviesService(IDbRepository<Movie> movies)
        {
            this.movies = movies;
        }

        public MoviesService(IDbRepository<Movie> moviesRepo, IIdentifierProvider identifierProvider)
            : this(moviesRepo)
        {
            this.identifierProvider = identifierProvider;
        }

        public MoviesService(
            IDbRepository<Movie> moviesRepo,
            IIdentifierProvider identifierProvider,
            IGenericDbRepository<ApplicationUser> usersRepo,
            IDbRepository<Review> reviewsRepo)
        {
            this.identifierProvider = identifierProvider;
            this.movies = moviesRepo;
            this.users = usersRepo;
            this.reviews = reviewsRepo;
        }

        public int Create(Movie movie)
        {
            this.movies.Add(movie);
            this.movies.Save();

            return movie.Id;
        }

        public int Edit(Movie movie)
        {
            this.movies.Update(movie);

            return movie.Id;
        }

        public void Delete(Movie movie)
        {
            this.movies.Delete(movie);
        }

        public IQueryable<Movie> GetAll()
        {
            return this.movies.All();
        }

        public IQueryable<Movie> PagedGetAll(int page, int size)
        {
            return this.movies.All()
                .OrderBy(m => m.Title)
                .ThenByDescending(m => m.Year)
                .Skip((page - 1) * size)
                .Take(size);
        }

        public IQueryable<Movie> GetTrending()
        {
            var lastWeek = DateTime.UtcNow.AddDays(-GlobalConstants.DefaultTrendingCacheDays);

            var trending = this.movies.All()
                .OrderByDescending(m => m.Reviews.Where(r => r.CreatedOn > lastWeek).Count())
                .OrderByDescending(m => m.Reviews.Select(r => r.Rating).Sum() / m.Reviews.Count())
                .OrderByDescending(m => m.CreatedOn)
                .Take(12);
            return trending;
        }

        public IQueryable<Movie> GetRecommended(string userId)
        {
            var user = this.users.GetById(userId);

            var top5ClosestUsers = this.users.All()
                                            .OrderByDescending(u => this.GetUserSimilarity(user, u))
                                            .Take(5);

            var highestRatedMovies = top5ClosestUsers
                                                .SelectMany(u => u.Reviews
                                                                    .Where(r => !user.WatchedList.Contains(r.Movie))
                                                                    .Select(r => r.Movie))
                                          .OrderByDescending(m => m.Reviews.Sum(r => r.Rating) / m.Reviews.Count())
                                          .Take(GlobalConstants.RecommendedMoviesCount / 2);

            var topInWatchListMovies = top5ClosestUsers
                                                .SelectMany(u => u.WatchList)
                                                .OrderByDescending(m => m.Reviews.Sum(r => r.Rating) / m.Reviews.Count())
                                                .Take(GlobalConstants.RecommendedMoviesCount / 2);

            var recommendedMovies = highestRatedMovies.Union(topInWatchListMovies);
            if (recommendedMovies.Count() == 0)
            {
                recommendedMovies = this.GetTrending();
            }

            return recommendedMovies;
        }

        public Movie GetById(string id)
        {
            var intId = this.identifierProvider.DecodeId(id);
            var movie = this.movies.GetById(intId);

            return movie;
        }

        public IQueryable<Movie> Search(string query, string country, string language, string genre)
        {
            var result = this.movies
                .All()
                .Where(m => (query != string.Empty) ? m.Title.ToLower().Contains(query.ToLower()) : true)
                .Where(m => (country != string.Empty) ? m.Country.Where(c => c.Name == country).Count() > 0 : true)
                .Where(m => (language != string.Empty) ? m.Language.Where(l => l.Name == language).Count() > 0 : true)
                .Where(m => (genre != string.Empty) ? m.Genre.Where(g => g.Name == genre).Count() > 0 : true)
                .OrderBy(m => m.Title)
                .ThenBy(m => m.Id);

            return result;
        }

        private double GetUserSimilarity(ApplicationUser currentUser, ApplicationUser anotherUser)
        {
            List<int> ageGroups = new List<int>(GlobalConstants.AgeGroups);
            ageGroups.Add(currentUser.Age);
            ageGroups.Add(anotherUser.Age);

            ageGroups.Sort();

            // check if in the same age group
            var ageSimilarity = ageGroups.IndexOf(currentUser.Age) - ageGroups.IndexOf(anotherUser.Age) == 0 ? 1 : 0;

            var genderSimilarity = currentUser.Gender == anotherUser.Gender ? 1 : 0;

            var countrySimilarity = currentUser.UserCountry == anotherUser.UserCountry ? 1 : 0;

            // movies both users have seen
            var watchedMoviesSimilarity = currentUser.WatchedList.Where(m => anotherUser.WatchedList.Contains(m))
                                                                 .Count() / currentUser.WatchedList.Count();

            // movies both userswant to see
            var watchListMoviesSimilarity = currentUser.WatchList.Where(m => anotherUser.WatchList.Contains(m))
                                                                 .Count() / currentUser.WatchList.Count();

            // movies both users rated highest
            var ratedMoviesSimilarities = currentUser.Reviews.Where(r1 => anotherUser.Reviews
                                                                                     .Where(r2 => (r2.MovieId == r1.MovieId) &&
                                                                                                   Math.Abs(r1.Rating - r2.Rating) < 2)
                                                                                      .Count() > 0)
                                                              .Count() / currentUser.Reviews.Count();

            var moviesTaste = watchedMoviesSimilarity * watchListMoviesSimilarity * ratedMoviesSimilarities;
            var demographic = (ageSimilarity + genderSimilarity + countrySimilarity) / 3;
            var totoalSimilarity = (0.70 * moviesTaste) + (0.30 * demographic);

            return totoalSimilarity;
        }
    }
}
