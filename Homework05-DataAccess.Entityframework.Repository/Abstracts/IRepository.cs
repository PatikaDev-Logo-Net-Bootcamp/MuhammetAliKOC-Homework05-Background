using Homework05_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework05_DataAccess.Entityframework.Repository.Abstracts
{
    public interface IRepository<T> where T : class
    {
        //T Get(int id);
        IQueryable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
