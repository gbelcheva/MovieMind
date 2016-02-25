namespace SentimentAnalyzer.Services.Data
{
    using System.Linq;
    using SentimentAnalyzer.Data.Models;

    public interface IWordsOccurrencesService
    {
        IQueryable<WordOccurrences> GetAll();

        void Create(WordOccurrences wordOccurrences);
    }
}
