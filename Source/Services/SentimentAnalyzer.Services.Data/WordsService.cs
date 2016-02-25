namespace SentimentAnalyzer.Services.Data
{
    using System;
    using System.Linq;
    using MovieMind.Data.Common;
    using SentimentAnalyzer.Data.Models;

    public class WordsService : IWordsService
    {
        private readonly IDbRepository<SentimentWord> words;

        public WordsService(IDbRepository<SentimentWord> words)
        {
            this.words = words;
        }

        public bool Any()
        {
            return this.words.All().Any();
        }

        public void Create(SentimentWord word)
        {
            this.words.Add(word);
            this.words.Save();
        }

        public IQueryable<SentimentWord> GetAll()
        {
            return this.words.All()
                .OrderBy(w => w.Word);
        }
    }
}
