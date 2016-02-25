namespace SentimentAnalyzer.Services.Data
{
    using System.Linq;
    using SentimentAnalyzer.Data.Models;

    public interface ICategoriesService
    {
        IQueryable<SentimentCategory> GetAll();

        bool Any();

        void Create(SentimentCategory category);
    }
}
