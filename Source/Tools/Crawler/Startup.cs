namespace Crawler
{
    using System;
    using Crawlers;
    using MovieMind.Data;
    using MovieMind.Data.Common;
    using MovieMind.Data.Models;
    using MovieMind.Services.Data;
    using MovieMind.Services.Data.MoviePropertiesServices;
    using SentimentAnalyzer.Data.Models;
    using SentimentAnalyzer.Services.Data;

    public static class Startup
    {
        public static void Main()
        {
            var db = new ApplicationDbContext();

            // repos
            var trainingReviewsRepo = new DbRepository<SentimentReview>(db);

            var moviesRepo = new DbRepository<Movie>(db);
            var peopleRepo = new DbRepository<MoviePerson>(db);
            var countriesRepo = new DbRepository<Country>(db);
            var languagesRepo = new DbRepository<Language>(db);
            var genresRepo = new DbRepository<Genre>(db);

            var trainingReviewsService = new TrainingReviewsService(trainingReviewsRepo);

            // services
            var moviesService = new MoviesService(moviesRepo);
            var peopleService = new MoviePeopleService(peopleRepo);
            var countriesService = new CountriesService(countriesRepo);
            var languagesService = new LanguagesService(languagesRepo);
            var genresService = new GenresService(genresRepo);

            // scrape data
            var reviewsCrawler = new ReviewDataCrawler(trainingReviewsService);
            reviewsCrawler.ScrapeDataToDb();
            Console.Clear();
            var movieCrawler = new MovieDataCrawler(moviesService, peopleService, languagesService, countriesService, genresService);
            movieCrawler.ScrapeDataToDb();
        }
    }
}
