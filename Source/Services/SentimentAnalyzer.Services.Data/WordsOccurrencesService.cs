namespace SentimentAnalyzer.Services.Data
{
    using System.Linq;
    using MovieMind.Data.Common;
    using SentimentAnalyzer.Data.Models;

    public class WordsOccurrencesService : IWordsOccurrencesService
    {
        private readonly IDbRepository<WordOccurrences> wordOccurrences;

        public WordsOccurrencesService(IDbRepository<WordOccurrences> wordOccurrences)
        {
            this.wordOccurrences = wordOccurrences;
        }

        public void Create(WordOccurrences word)
        {
            this.wordOccurrences.Add(word);
            this.wordOccurrences.Save();
        }

        public IQueryable<WordOccurrences> GetAll()
        {
            return this.wordOccurrences.All()
                .OrderBy(w => w.Word);
        }
    }
}
