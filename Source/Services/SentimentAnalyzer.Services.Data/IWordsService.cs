namespace SentimentAnalyzer.Services.Data
{
    using System.Linq;
    using SentimentAnalyzer.Data.Models;

    public interface IWordsService
    {
        IQueryable<SentimentWord> GetAll();

        bool Any();

        void Create(SentimentWord word);
    }
}
