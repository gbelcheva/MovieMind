namespace MovieMind.Data.Models
{
    using System.Collections.Generic;
    using Common.Models;

    public class Movie : BaseModel<int>
    {
        public Movie()
        {
            this.Reviews = new HashSet<Review>();
            this.Genre = new HashSet<Genre>();
            this.Writer = new HashSet<MoviePerson>();
            this.Director = new HashSet<MoviePerson>();
            this.Actors = new HashSet<MoviePerson>();
            this.Language = new HashSet<Language>();
            this.Country = new HashSet<Country>();
        }

        public string Title { get; set; }

        public int? Year { get; set; }

        public string Rated { get; set; }

        public string Runtime { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<Genre> Genre { get; set; }

        public virtual ICollection<MoviePerson> Director { get; set; }

        public virtual ICollection<MoviePerson> Writer { get; set; }

        public virtual ICollection<MoviePerson> Actors { get; set; }

        public string Plot { get; set; }

        public virtual ICollection<Language> Language { get; set; }

        public virtual ICollection<Country> Country { get; set; }

        public string Awards { get; set; }

        public string Poster { get; set; }

        public double? ImdbRating { get; set; }

        public string ImdbId { get; set; }

        public int? ImdbVotes { get; set; }
    }
}
