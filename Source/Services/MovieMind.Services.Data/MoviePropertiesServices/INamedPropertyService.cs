namespace MovieMind.Services.Data.MoviePropertiesServices
{
    using System.Linq;

    using MovieMind.Data.Models;

    public interface INamedPropertyService<T>
    {
        IQueryable<T> GetAll();

        T EnsureProperty(string name);
    }
}
