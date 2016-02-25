namespace MovieMind.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Data.Models;
    using Infrastructure.Mapping;
    using Microsoft.AspNet.Identity;
    using Services.Data;
    using Services.Data.MoviePropertiesServices;
    using ViewModels.Movies;
    using ViewModels.Reviews;
    public class MoviesController : BaseController
    {
        private const int SearchPageSize = 9;
        private const int ReviewsPrePage = 4;
        private readonly IMoviesService movies;
        private readonly IReviewsService reviews;
        private readonly INamedPropertyService<Language> languages;
        private readonly INamedPropertyService<Country> countries;
        private readonly INamedPropertyService<Genre> genres;

        public MoviesController(
            IMoviesService movies,
            IReviewsService reviews,
            INamedPropertyService<Language> languages,
            INamedPropertyService<Country> countries,
            INamedPropertyService<Genre> genres)
        {
            this.movies = movies;
            this.reviews = reviews;
            this.languages = languages;
            this.countries = countries;
            this.genres = genres;
        }

        public PartialViewResult ById(string id)
        {
            id = id.Split('-')[0];
            var movie = this.movies.GetById(id);
            var viewModel = this.Mapper.Map<MovieDetailsViewModel>(movie);

            return this.PartialView("_MovieDetails", viewModel);
        }

        public ActionResult RecommendedForYou()
        {
            var id = this.User.Identity.GetUserId();

            var movie = this.movies.GetRecommended(id);
            var viewModel = this.Mapper.Map<MovieDetailsViewModel>(movie);

            return this.View(viewModel);
        }

        public ActionResult MovieById(string id = "id")
        {
            id = id.Split('-')[0];
            var movie = this.movies.GetById(id);
            var movieDetails = this.Mapper.Map<MovieDetailsViewModel>(movie);
            var movieReviews = this.reviews
                .GetAll()
                .Where(r => r.MovieId == movie.Id)
                .OrderByDescending(r => r.CreatedOn)
                .OrderByDescending(r => r.Rating)
                .To<ReviewViewModel>()
                .ToList();
            //var reviews = movie.Reviews.Select(r => this.Mapper.Map<ReviewViewModel>(r)).ToList();

            var viewModel = new MovieByIdViewModel()
            {
                Movie = movieDetails,
                Reviews = new PagedReviewsViewModel()
                {
                    CurrentPage = 1,
                    TotalPages = movieReviews.Count / ReviewsPrePage,
                    Reviews = movieReviews
                },
                PostReview = new ReviewPostModel() { MovieId = id }
            };

            return this.View(viewModel);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var movies = this.movies
                .PagedGetAll(1, SearchPageSize)
                .To<MovieViewModel>()
                .ToList();

            int totalPages = (int)Math.Ceiling(this.movies.GetAll().Count() / (decimal)SearchPageSize);

            this.CheckIfCacheExpired();

            var result = new PagedMoviesViewModel()
            {
                CurrentPage = 1,
                TotalPages = totalPages,
                Movies = movies,
                AllLanguages = (List<Language>)this.HttpContext.Cache["languages"],
                AllCountries = (List<Country>)this.HttpContext.Cache["countries"],
                AllGenres = (List<Genre>)this.HttpContext.Cache["genres"]
            };

            return this.View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult Index(string query, string country, string language, string genre, int page = 1)
        {
            var foundMovies = this.movies
                .Search(query, country, language, genre)
                .Skip((page - 1) * SearchPageSize)
                .Take(SearchPageSize)
                .To<MovieViewModel>()
                .ToList();

            int totalPages = (int)Math.Ceiling(this.movies.Search(query, country, language, genre).Count() / (decimal)SearchPageSize);

            this.CheckIfCacheExpired();

            var result = new PagedMoviesViewModel()
            {
                CurrentPage = page,
                TotalPages = totalPages,
                Movies = foundMovies,
                Query = query,
                Language = language,
                Country = country,
                Genre = genre,
                AllLanguages = (List<Language>)this.HttpContext.Cache["languages"],
                AllCountries = (List<Country>)this.HttpContext.Cache["countries"],
                AllGenres = (List<Genre>)this.HttpContext.Cache["genres"]
            };

            return this.PartialView("_MovieResults", result);
        }

        private void CheckIfCacheExpired()
        {
            if (this.HttpContext.Cache["languages"] == null)
            {
                this.HttpContext.Cache["languages"] = this.languages.GetAll().OrderBy(l => l.Name).ToList();
            }

            if (this.HttpContext.Cache["countries"] == null)
            {
                this.HttpContext.Cache["countries"] = this.countries.GetAll().OrderBy(c => c.Name).ToList();
            }

            if (this.HttpContext.Cache["genres"] == null)
            {
                this.HttpContext.Cache["genres"] = this.genres.GetAll().OrderBy(g => g.Name).ToList();
            }
        }
    }
}
