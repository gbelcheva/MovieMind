namespace MovieMind.Data.Models
{
    using System.Collections.Generic;
    using Common.Models;

    public class Genre : BaseModel<int>
    {
        public Genre()
        {
            this.Movies = new HashSet<Movie>();
        }

        public string Name { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
