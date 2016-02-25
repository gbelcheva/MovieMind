namespace SentimentAnalyzer.Data.Models
{
    using MovieMind.Data.Common.Models;

    public class WordOccurrences : BaseModel<int>
    {
        public string Word { get; set; }

        public int Occurrences { get; set; }

        public int CategoryId { get; set; }

        public SentimentCategory Category { get; set; }
    }
}