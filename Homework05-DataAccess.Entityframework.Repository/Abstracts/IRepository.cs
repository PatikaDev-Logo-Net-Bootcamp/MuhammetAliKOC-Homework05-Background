using System.Collections.Generic;
using System.Linq;

namespace Homework05_DataAccess.Entityframework.Repository.Abstracts
{
    public interface IRepository<T> where T : class
    {
        //T Get(int id);
        IQueryable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

        void AddRange(List<T> entities);
        void UpdateRange(List<T> entities);
        void DeleteRange(List<T> entities);
    }
}
