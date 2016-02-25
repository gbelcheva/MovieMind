namespace SentimentAnalyzer.Services.Data
{
    using System.Linq;
    using MovieMind.Data.Common;
    using SentimentAnalyzer.Data.Models;

    public class TestingReviewsService : ITestingReviewsService
    {
        private readonly IDbRepository<SentimentTestingReview> reviews;

        public TestingReviewsService(IDbRepository<SentimentTestingReview> reviews)
        {
            this.reviews = reviews;
        }

        public void Create(SentimentTestingReview review)
        {
            this.reviews.Add(review);
            this.reviews.Save();
        }

        public IQueryable<SentimentTestingReview> GetAll()
        {
            return this.reviews.All()
                .OrderBy(r => r.Content);
        }
    }
}
