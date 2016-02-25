namespace MovieMind.Services.Data.MoviePropertiesServices
{
    using System.Linq;

    using MovieMind.Data.Common;
    using MovieMind.Data.Models;

    public class MoviePeopleService : INamedPropertyService<MoviePerson>
    {
        private readonly IDbRepository<MoviePerson> people;

        public MoviePeopleService(IDbRepository<MoviePerson> peopleRepo)
        {
            this.people = peopleRepo;
        }

        public MoviePerson EnsureProperty(string name)
        {
            var person = this.people.All().FirstOrDefault(x => x.FullName == name);
            if (person != null)
            {
                return person;
            }

            person = new MoviePerson { FullName = name };
            this.people.Add(person);
            this.people.Save();

            return person;
        }

        public IQueryable<MoviePerson> GetAll()
        {
            return this.people.All().OrderBy(x => x.FullName);
        }
    }
}
