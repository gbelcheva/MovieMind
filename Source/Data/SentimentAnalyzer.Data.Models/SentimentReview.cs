namespace SentimentAnalyzer.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MovieMind.Data.Common.Models;

    public class SentimentReview : BaseModel<int>
    {
        public SentimentReview()
        {
            this.Words = new HashSet<ReviewWord>();
        }

        [Required]
        [Range(1, 10)]
        public double Rating { get; set; }

        [Required]
        public string Content { get; set; }

        public virtual ICollection<ReviewWord> Words { get; set; }
    }
}
