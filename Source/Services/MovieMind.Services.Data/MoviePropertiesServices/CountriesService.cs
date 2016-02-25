namespace MovieMind.Services.Data.MoviePropertiesServices
{
    using System.Linq;

    using MovieMind.Data.Common;
    using MovieMind.Data.Models;

    public class CountriesService : INamedPropertyService<Country>
    {
        private readonly IDbRepository<Country> countries;

        public CountriesService(IDbRepository<Country> countriesRepo)
        {
            this.countries = countriesRepo;
        }

        public Country EnsureProperty(string name)
        {
            var category = this.countries.All().FirstOrDefault(x => x.Name == name);
            if (category != null)
            {
                return category;
            }

            category = new Country { Name = name };
            this.countries.Add(category);
            this.countries.Save();

            return category;
        }

        public IQueryable<Country> GetAll()
        {
            return this.countries.All().OrderBy(x => x.Name);
        }
    }
}
