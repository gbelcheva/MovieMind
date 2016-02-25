namespace SentimentAnalyzer.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MovieMind.Data.Common.Models;

    public class ReviewWord : BaseModel<int>
    {
        public virtual SentimentReview SentimentReview { get; set; }

        public int SentimentReviewId { get; set; }

        [Required]
        public string Word { get; set; }

        [Required]
        [Range(-1, 1)]
        public int Polarity { get; set; }
    }
}