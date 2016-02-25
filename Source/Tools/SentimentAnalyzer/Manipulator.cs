namespace Tools.SentimentAnalyzer
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using global::SentimentAnalyzer.Data.Models;
    using global::SentimentAnalyzer.Services.Data;

    public abstract class ReviewManipulator
    {

        //// protected readonly char[] punctuation = { ' ', '.', ',', '?', '!', ':', ';', '-', '(', ')', '[', ']', '{', '}', '\'', '`', '\"', '&', '*', '=', '+', '#', '~', '%', '@', '/', '\\' };
        protected readonly IWordsService SentimentWords;
        protected readonly ICategoriesService SentimentCategories;

        public ReviewManipulator(
            IWordsService words,
            ICategoriesService categories)
        {
            this.SentimentWords = words;
            if (!this.SentimentWords.GetAll().Any())
            {
                DataParser.ParseVocabularyToDb(this.SentimentWords);
            }

            this.SentimentCategories = categories;
        }

        protected SentimentReview PreprocessReview(SentimentReview review, IEnumerable<SentimentWord> vocabulary)
        {
            foreach (var w in vocabulary)
            {
                if (Regex.IsMatch(review.Content, @"([^a-zA-Z0-9_])(" + w.Word + ")([^a-zA-Z0-9_])"))
                {
                    review.Words.Add(new ReviewWord() { Word = w.Word, Polarity = w.Polarity});
                }
            }

            return review;
        }
    }
}
