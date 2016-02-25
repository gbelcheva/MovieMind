namespace Tools.SentimentAnalyzer
{
    using System.Collections.Generic;
    using System.Linq;
    using global::SentimentAnalyzer.Data.Models;
    using global::SentimentAnalyzer.Services.Data;

    public class ReviewTrainer : ReviewManipulator
    {
        private readonly ITrainingReviewsService reviews;

        public ReviewTrainer(
            IWordsService words,
            ICategoriesService categories,
            ITrainingReviewsService reviews)
            : base(words, categories)
        {
            this.reviews = reviews;
        }

        public void TrainAnalyzer()
        {
            var reviews = this.PreprocessReviews();

            foreach (var review in reviews)
            {
                var foundCategory = this.SentimentCategories.GetAll().FirstOrDefault(c => c.Name == review.Rating.ToString());

                if (foundCategory != null)
                {
                    this.UpdateCategory(foundCategory, review);
                }
                else
                {
                    this.CreateCategory(review);
                }
            }
        }

        private void CreateCategory(SentimentReview review)
        {
            var newCategory = new SentimentCategory()
            {
                Name = review.Rating.ToString(),
                TrainSampleSize = 1
            };

            foreach (var word in review.Words)
            {
                newCategory.WordOccurrences.Add(new WordOccurrences() { Word = word.Word, Occurrences = 1 });
            }

            this.SentimentCategories.Create(
                newCategory
            );

        }

        private void UpdateCategory(SentimentCategory foundCategory, SentimentReview review)
        {
            foundCategory.TrainSampleSize++;

            foreach (var word in review.Words)
            {
                if (foundCategory.WordOccurrences.FirstOrDefault(wo => wo.Word == word.Word) != null)
                {
                    foundCategory.WordOccurrences.FirstOrDefault(wo => wo.Word == word.Word).Occurrences++;
                }
                else
                {
                    foundCategory.WordOccurrences.Add(new WordOccurrences() { Word = word.Word, Occurrences = 1 });
                }
            }
        }

        private IEnumerable<SentimentReview> PreprocessReviews()
        {
            var vocabulary = this.SentimentWords.GetAll().ToList();

            var allReviews = this.reviews.GetAll().Take(500).ToList();

            for (int i = 0; i < allReviews.Count; i++)
            {
                allReviews[i] = this.PreprocessReview(allReviews[i], vocabulary);
            }

            return allReviews;
        }
    }
}
