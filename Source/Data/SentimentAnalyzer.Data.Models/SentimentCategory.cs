namespace SentimentAnalyzer.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MovieMind.Data.Common.Models;

    public class SentimentCategory : BaseModel<int>
    {
        public SentimentCategory()
        {
            this.WordOccurrences = new HashSet<WordOccurrences>();
        }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0, 9999999)]
        public int TrainSampleSize { get; set; }

        public virtual ICollection<WordOccurrences> WordOccurrences { get; set; }
    }
}
