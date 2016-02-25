
namespace MovieMind.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;
    using Infrastructure.Mapping;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using MovieMind.Data.Models;
    using MovieMind.Services.Data;
    using MovieMind.Web.Areas.Administration.Models;

    public class MoviesController : Controller
    {
        private readonly IMoviesService movies;

        public MoviesController(IMoviesService movies)
        {
            this.movies = movies;
        }

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult Movies_Read([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult result = this.movies.GetAll()
                .ToDataSourceResult(request);

            return this.Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Movies_Create([DataSourceRequest]DataSourceRequest request, Movie movie)
        {
            if (this.ModelState.IsValid)
            {
                var entity = new Movie
                {
                    Title = movie.Title,
                    Year = movie.Year,
                    Rated = movie.Rated,
                    Runtime = movie.Runtime,
                    Plot = movie.Plot,
                    Awards = movie.Awards,
                    Poster = movie.Poster,
                    ImdbRating = movie.ImdbRating,
                    ImdbId = movie.ImdbId,
                    ImdbVotes = movie.ImdbVotes,
                    CreatedOn = movie.CreatedOn,
                    ModifiedOn = movie.ModifiedOn,
                    IsDeleted = movie.IsDeleted,
                    DeletedOn = movie.DeletedOn
                };

                this.movies.Create(entity);
                movie.Id = entity.Id;
            }

            return this.Json(new[] { movie }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Movies_Update([DataSourceRequest]DataSourceRequest request, Movie movie)
        {
            if (this.ModelState.IsValid)
            {
                var entity = new Movie
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Year = movie.Year,
                    Rated = movie.Rated,
                    Runtime = movie.Runtime,
                    Plot = movie.Plot,
                    Awards = movie.Awards,
                    Poster = movie.Poster,
                    ImdbRating = movie.ImdbRating,
                    ImdbId = movie.ImdbId,
                    ImdbVotes = movie.ImdbVotes,
                    CreatedOn = movie.CreatedOn,
                    ModifiedOn = movie.ModifiedOn,
                    IsDeleted = movie.IsDeleted,
                    DeletedOn = movie.DeletedOn
                };

                this.movies.Edit(entity);
            }

            return this.Json(new[] { movie }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Movies_Destroy([DataSourceRequest]DataSourceRequest request, Movie movie)
        {
            this.movies.Delete(movie);

            return this.Json(new[] { movie }.ToDataSourceResult(request, ModelState));
        }
    }
}
