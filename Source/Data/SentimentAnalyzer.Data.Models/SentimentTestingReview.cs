namespace SentimentAnalyzer.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using MovieMind.Data.Common.Models;

    public class SentimentTestingReview : BaseModel<int>
    {
        [Required]
        public string Content { get; set; }
    }
}
