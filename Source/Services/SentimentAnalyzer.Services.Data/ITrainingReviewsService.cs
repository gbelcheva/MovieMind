namespace SentimentAnalyzer.Services.Data
{
    using System.Linq;
    using SentimentAnalyzer.Data.Models;

    public interface ITrainingReviewsService
    {
        IQueryable<SentimentReview> GetAll();

        void Create(SentimentReview review);
    }
}
