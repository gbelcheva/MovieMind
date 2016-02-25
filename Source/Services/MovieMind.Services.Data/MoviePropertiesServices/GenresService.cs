namespace MovieMind.Services.Data.MoviePropertiesServices
{
    using System.Linq;

    using MovieMind.Data.Common;
    using MovieMind.Data.Models;

    public class GenresService : INamedPropertyService<Genre>
    {
        private readonly IDbRepository<Genre> genres;

        public GenresService(IDbRepository<Genre> genresRepo)
        {
            this.genres = genresRepo;
        }

        public Genre EnsureProperty(string name)
        {
            var genre = this.genres.All().FirstOrDefault(x => x.Name == name);
            if (genre != null)
            {
                return genre;
            }

            genre = new Genre { Name = name };
            this.genres.Add(genre);
            this.genres.Save();

            return genre;
        }

        public IQueryable<Genre> GetAll()
        {
            return this.genres.All().OrderBy(x => x.Name);
        }
    }
}
