namespace SentimentAnalyzer.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MovieMind.Data.Common.Models;

    public class SentimentWord : BaseModel<int>
    {
        public SentimentWord()
        {
            this.WordOccurrences = new HashSet<WordOccurrences>();
        }

        public ICollection<WordOccurrences> WordOccurrences { get; set; }

        [Required]
        public string Word { get; set; }

        [Required]
        [Range(-1, 1)]
        public int Polarity { get; set; }
    }
}
