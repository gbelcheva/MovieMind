namespace SentimentAnalyzer.Services.Data
{
    using System;
    using System.Linq;
    using MovieMind.Data.Common;
    using SentimentAnalyzer.Data.Models;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDbRepository<SentimentCategory> categories;

        public CategoriesService(IDbRepository<SentimentCategory> categories)
        {
            this.categories = categories;
        }

        public bool Any()
        {
            return this.categories.All().Any();
        }

        public void Create(SentimentCategory category)
        {
            this.categories.Add(category);
            this.categories.Save();
        }

        public IQueryable<SentimentCategory> GetAll()
        {
            return this.categories.All()
                .OrderBy(c => c.Name);
        }
    }
}
