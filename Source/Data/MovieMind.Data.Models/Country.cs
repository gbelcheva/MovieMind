namespace MovieMind.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common.Models;

    public class Country : BaseModel<int>
    {
        public Country()
        {
            this.Movies = new HashSet<Movie>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
