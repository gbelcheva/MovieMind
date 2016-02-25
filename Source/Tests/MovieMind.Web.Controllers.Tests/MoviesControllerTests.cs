namespace MovieMind.Web.Controllers.Tests
{
    using Moq;

    using MovieMind.Data.Models;
    using MovieMind.Services.Data;
    using MovieMind.Web.Infrastructure.Mapping;
    using MovieMind.Web.ViewModels.Home;

    using NUnit.Framework;
    using Services.Data.MoviePropertiesServices;
    using TestStack.FluentMVCTesting;
    using ViewModels.Movies;

    [TestFixture]
    public class MoviesControllerTests
    {
        [Test]
        public void ByIdShouldWorkCorrectly()
        {
            var autoMapperConfig = new AutoMapperConfig();
            autoMapperConfig.Execute(typeof(MoviesController).Assembly);
            const string MovieTitle = "SomeContent";
            const string MoviePoster = "poster";
            const int MovieYear = 1999;

            var moviesServiceMock = new Mock<IMoviesService>();
            moviesServiceMock.Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(new Movie { Title = MovieTitle, Year = MovieYear, Poster = MoviePoster});

            var reviewsServiceMock = new Mock<IReviewsService>();

            var languagesServiceMock = new Mock<INamedPropertyService<Language>>();
            var countriesServiceMock = new Mock<INamedPropertyService<Country>>();
            var genreServiceMock = new Mock<INamedPropertyService<Genre>>();

            moviesServiceMock.Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(new Movie { Title = MovieTitle, Year = 1999, Poster = "dsgre", Plot = "dsfsdlkfj" });

            var controller = new MoviesController(moviesServiceMock.Object, reviewsServiceMock.Object, languagesServiceMock.Object, countriesServiceMock.Object, genreServiceMock.Object));
            controller.WithCallTo(x => x.MovieById("asdasasd"))
                .ShouldRenderPartialView("_MovieDetails")
                .WithModel<MovieDetailsViewModel>(
                    viewModel =>
                        {
                            Assert.AreEqual(MovieTitle, MovieYear, MoviePoster);
                        }).AndNoModelErrors();
        }
    }
}
