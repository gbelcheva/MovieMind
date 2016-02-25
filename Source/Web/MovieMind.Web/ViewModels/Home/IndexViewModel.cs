namespace MovieMind.Web.ViewModels.Home
{
    using System.Collections.Generic;
    using Movies;

    public class IndexViewModel
    {
        public int TotalMovies { get; set; }

        public int TotalReviews { get; set; }

        public IEnumerable<MovieViewModel> Movies { get; set; }
    }
}
