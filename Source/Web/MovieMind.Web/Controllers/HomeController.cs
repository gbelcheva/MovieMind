namespace MovieMind.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Infrastructure.Mapping;

    using Services.Data;
    using ViewModels.Home;
    using ViewModels.Movies;

    public class HomeController : BaseController
    {
        private readonly IMoviesService movies;
        private readonly IReviewsService reviews;

        public HomeController(IMoviesService movies, IReviewsService reviews)
        {
            this.movies = movies;
            this.reviews = reviews;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (this.HttpContext.Cache["Trending"] == null)
            {
                var trendingMovies = this.movies.GetTrending().To<MovieViewModel>().ToList();
                var viewModel = new IndexViewModel()
                {
                    Movies = trendingMovies,
                    TotalMovies = this.movies.GetAll().Count(),
                    TotalReviews = this.reviews.GetAll().Count()
                };

                this.HttpContext.Cache.Insert(
                    "Trending",
                    viewModel,
                    null,
                    DateTime.UtcNow.AddDays(7),
                    TimeSpan.Zero);
            }

            return this.View(this.HttpContext.Cache["Trending"]);
        }
    }
}
