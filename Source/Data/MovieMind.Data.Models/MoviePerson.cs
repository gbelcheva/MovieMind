namespace MovieMind.Data.Models
{
    using System.Collections.Generic;
    using Common.Models;

    public class MoviePerson : BaseModel<int>
    {
        public MoviePerson()
        {
            this.Movies = new HashSet<Movie>();
        }

        public string FullName { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
