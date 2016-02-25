namespace MovieMind.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Common.Models;

    public class Review : BaseModel<int>
    {
        public virtual string AuthorId { get; set; }

        [Required]
        public virtual ApplicationUser Author { get; set; }

        public virtual int MovieId { get; set; }

        [Required]
        public virtual Movie Movie { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [Range(1, 10)]
        public double Rating { get; set; }
    }
}
