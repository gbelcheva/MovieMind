namespace MovieMind.Web.ViewModels.Movies
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using MovieMind.Data.Models;
    using MovieMind.Web.Infrastructure.Mapping;

    public class PagedMoviesViewModel
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public string Query { get; set; }

        public string Language { get; set; }

        public string Country { get; set; }

        public string Genre { get; set; }

        public IEnumerable<Language> AllLanguages { get; set; }

        public IEnumerable<Country> AllCountries { get; set; }

        public IEnumerable<Genre> AllGenres { get; set; }

        public IEnumerable<MovieViewModel> Movies { get; set; }
    }
}
