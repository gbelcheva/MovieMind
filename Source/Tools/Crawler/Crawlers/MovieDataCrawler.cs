namespace Crawler.Crawlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using MovieMind.Common;
    using MovieMind.Data.Models;
    using MovieMind.Services.Data;
    using MovieMind.Services.Data.MoviePropertiesServices;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class MovieDataCrawler
    {
        private const int StartMovie = 1500000;
        private const int EndMovie = 18010;
        private const string StatusMessageMovies = "Scraping movies {0}/{1}";
        private const string FinishedMoviesMessage = "\nFinished!\nTotal Movies: {0}\n";
        private const string MoviesPath = "../../movies.json";
        private const string MovieUrlPattern = "http://www.omdbapi.com/?i=tt{0}&plot=short&r=json";

        private readonly IMoviesService movies;
        private readonly INamedPropertyService<MoviePerson> people;
        private readonly INamedPropertyService<Language> languages;
        private readonly INamedPropertyService<Country> countries;
        private readonly INamedPropertyService<Genre> genres;

        public MovieDataCrawler(
            IMoviesService movies,
            INamedPropertyService<MoviePerson> people,
            INamedPropertyService<Language> languages,
            INamedPropertyService<Country> countries,
            INamedPropertyService<Genre> genres)
        {
            this.movies = movies;
            this.people = people;
            this.languages = languages;
            this.countries = countries;
            this.genres = genres;
        }

        public static void ScrapeDataToJson()
        {
            List<string> allMovies = new List<string>();

            for (int i = StartMovie; i >= EndMovie; i--)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write(
                    StatusMessageMovies,
                    -(i - StartMovie + 1),
                    -(EndMovie - StartMovie + 1));

                var curId = i.ToString().PadLeft(7, '0');
                var curUrl = string.Format(MovieUrlPattern, curId);
                HttpWebRequest request = WebRequest.Create(curUrl) as HttpWebRequest;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var encoding = ASCIIEncoding.ASCII;
                var jsonString = string.Empty;
                using (var reader = new StreamReader(response.GetResponseStream(), encoding))
                {
                    jsonString = reader.ReadToEnd();
                }

                if (!jsonString.Contains("\"Response\":\"False\"") &&
                    !jsonString.Contains("\"Poster\":\"N/A\"") &&
                    jsonString.Contains("\"Type\":\"movie\""))
                {
                    allMovies.Add(jsonString);
                }
            }

            string json = JsonConvert.SerializeObject(allMovies.ToArray());
            File.WriteAllText(MoviesPath, json);

            Console.WriteLine(FinishedMoviesMessage, allMovies.Count);
        }

        public void ScrapeDataToDb()
        {
            var countriesList = new HashSet<string>();
            StringBuilder sb = new StringBuilder();

            for (int i = StartMovie; i >= EndMovie; i--)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write(
                    StatusMessageMovies,
                    -(i - StartMovie + 1),
                    -(EndMovie - StartMovie + 1));

                var curId = i.ToString().PadLeft(7, '0');
                var curUrl = string.Format(MovieUrlPattern, curId);

                HttpWebRequest request = WebRequest.Create(curUrl) as HttpWebRequest;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var encoding = Encoding.UTF8;
                var jsonString = string.Empty;
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    jsonString = reader.ReadToEnd();
                }

                jsonString = EscapeBackSlash(jsonString);
                if (!jsonString.Contains("\"Response\":\"False\"") &&
                    !jsonString.Contains("\"Poster\":\"N/A\"") &&
                    jsonString.Contains("\"Type\":\"movie\""))
                {
                    JObject movieJObject = JObject.Parse(jsonString);

                    this.movies.Create(this.JObjectToMovie(movieJObject));
                }
            }
        }

        private static string EscapeBackSlash(string input)
        {
            var pattern = @"(?<=[^(\\)])\\(?=[^(\\|""|\/|b|f|n|r|t)])";

            return Regex.Replace(input, pattern, "\\\\");
        }

        private static List<string> StringToList(string inputString)
        {
            return inputString
                .Split(new string[] { ", ", ",", " , ", " ," }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }

        private Movie JObjectToMovie(JObject movieObject)
        {
            var movie = new Movie()
            {
                Title = movieObject["Title"].ToString(),
                Year = movieObject["Year"].ToString().ToInt(),
                Rated = movieObject["Rated"].ToString(),
                Runtime = movieObject["Runtime"].ToString(),
                Awards = movieObject["Awards"].ToString(),
                Plot = movieObject["Plot"].ToString(),
                ImdbRating = movieObject["imdbRating"].ToString().ToDouble(),
                ImdbId = movieObject["imdbID"].ToString(),
                ImdbVotes = movieObject["imdbVotes"].ToString().Replace(",", string.Empty).ToInt(),
                Poster = movieObject["Poster"].ToString() == "N/A" ? GlobalConstants.DefaultPosterUrl : movieObject["Poster"].ToString()
            };

            var writersStrings = StringToList(movieObject["Writer"].ToString());
            foreach (var w in writersStrings)
            {
                if (w == "N/A")
                {
                    continue;
                }

                var person = this.people.EnsureProperty(w);
                movie.Writer.Add(person);
            }

            var directorsStrings = StringToList(movieObject["Director"].ToString());
            foreach (var d in directorsStrings)
            {
                if (d == "N/A")
                {
                    continue;
                }

                var director = this.people.EnsureProperty(d);
                movie.Director.Add(director);
            }

            var actorsStrings = StringToList(movieObject["Actors"].ToString());
            foreach (var a in actorsStrings)
            {
                if (a == "N/A")
                {
                    continue;
                }

                var actors = this.people.EnsureProperty(a);
                movie.Actors.Add(actors);
            }

            var genresStrings = StringToList(movieObject["Genre"].ToString());
            foreach (var g in genresStrings)
            {
                if (g == "N/A")
                {
                    continue;
                }

                var genre = this.genres.EnsureProperty(g);
                movie.Genre.Add(genre);
            }

            var languagesStrings = StringToList(movieObject["Language"].ToString());
            foreach (var l in languagesStrings)
            {
                if (l == "N/A")
                {
                    continue;
                }

                var language = this.languages.EnsureProperty(l);
                movie.Language.Add(language);
            }

            var countriesStrings = StringToList(movieObject["Country"].ToString());
            foreach (var c in countriesStrings)
            {
                if (c == "N/A")
                {
                    continue;
                }

                var country = this.countries.EnsureProperty(c);
                movie.Country.Add(country);
            }

            return movie;
        }
    }
}
