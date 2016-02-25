namespace Tools.SentimentAnalyzer
{
    using System.Collections.Generic;
    using System.Linq;
    using global::SentimentAnalyzer.Data.Models;
    using global::SentimentAnalyzer.Services.Data;

    public class ReviewAnalyzer : ReviewManipulator
    {
        public ReviewAnalyzer(
            IWordsService words,
            ICategoriesService categories)
            : base(words, categories)
        {
        }

        public ReviewAnalyzer(
            IWordsService words,
            ICategoriesService categories,
            ITrainingReviewsService reviews)
            : base(words, categories)
        {
            var reviewTrainer = new ReviewTrainer(words, categories, reviews);
            reviewTrainer.TrainAnalyzer();
        }

        public string AnalyzeReview(string content)
        {
            var vocabulary = this.SentimentWords.GetAll().ToList();
            var inputReview = PreprocessReview(new SentimentReview() { Content = content }, vocabulary);

            var categoriesProbabilities = this.GetCategoriesProbabilities(inputReview);

            var mostProbableCategory = categoriesProbabilities
                .OrderByDescending(cp => cp.Value)
                .FirstOrDefault();

            return mostProbableCategory.Key;
        }

        private Dictionary<string, int> GetCategoriesProbabilities(SentimentReview review)
        {
            var categoriesProbabilities = new Dictionary<string, int>();
            var allCategories = this.SentimentCategories.GetAll().ToList();

            foreach (var category in allCategories)
            {
                int posteriorRatingProbability = this.ApplyBeyesTheorem(allCategories, review, category);

                categoriesProbabilities.Add(category.Name, posteriorRatingProbability);
            }

            return categoriesProbabilities;
        }

        private int ApplyBeyesTheorem(List<SentimentCategory> categories, SentimentReview review, SentimentCategory category)
        {
            int aprioriRatingProbability = category.TrainSampleSize / categories.Sum(c => c.TrainSampleSize);

            int posteriorWordsProbability = 1;

            foreach (var word in review.Words)
            {
                if (category.WordOccurrences.FirstOrDefault(wo => wo.Word == word.Word) != null)
                {
                    posteriorWordsProbability *= category.WordOccurrences.FirstOrDefault(wo => wo.Word == word.Word).Occurrences;
                }
            }

            int posteriorRatingProbability = aprioriRatingProbability * posteriorWordsProbability;
            return posteriorRatingProbability;
        }
    }
}
