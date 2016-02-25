namespace SentimentAnalyzer.Services.Data
{
    using System.Linq;
    using MovieMind.Data.Common;
    using SentimentAnalyzer.Data.Models;

    public class TrainingReviewsService : ITrainingReviewsService
    {
        private readonly IDbRepository<SentimentReview> reviews;

        public TrainingReviewsService(IDbRepository<SentimentReview> reviews)
        {
            this.reviews = reviews;
        }

        public void Create(SentimentReview review)
        {
            this.reviews.Add(review);
            this.reviews.Save();
        }

        public IQueryable<SentimentReview> GetAll()
        {
            return this.reviews.All()
                .OrderBy(r => r.Rating)
                .ThenBy(r => r.Content);
        }
    }
}
