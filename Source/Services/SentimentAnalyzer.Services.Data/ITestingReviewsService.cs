namespace SentimentAnalyzer.Services.Data
{
    using System.Linq;
    using SentimentAnalyzer.Data.Models;

    public interface ITestingReviewsService
    {
        IQueryable<SentimentTestingReview> GetAll();

        void Create(SentimentTestingReview review);
    }
}
