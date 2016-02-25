namespace MovieMind.Data.Common
{
    using System.Linq;

    using MovieMind.Data.Common.Models;

    public interface IGenericDbRepository<T> : IGenericDbRepository<T, string>
        where T : class
    {
    }

    public interface IGenericDbRepository<T, in TKey>
        where T : class
    {
        IQueryable<T> All();

        T GetById(TKey id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Save();
    }
}
