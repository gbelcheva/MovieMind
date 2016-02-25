namespace MovieMind.Services.Data.MoviePropertiesServices
{
    using System.Linq;

    using MovieMind.Data.Common;
    using MovieMind.Data.Models;

    public class LanguagesService : INamedPropertyService<Language>
    {
        private readonly IDbRepository<Language> languages;

        public LanguagesService(IDbRepository<Language> languagesRepo)
        {
            this.languages = languagesRepo;
        }

        public Language EnsureProperty(string name)
        {
            var language = this.languages.All().FirstOrDefault(x => x.Name == name);
            if (language != null)
            {
                return language;
            }

            language = new Language { Name = name };
            this.languages.Add(language);
            this.languages.Save();

            return language;
        }

        public IQueryable<Language> GetAll()
        {
            return this.languages.All().OrderBy(x => x.Name);
        }
    }
}
