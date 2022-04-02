using Homework05_DataAccess.Entityframework.Repository.Abstracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Homework05_DataAccess.Entityframework.Repository.Concretes
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public readonly IUnitOfWork unitOfWork;
        public Repository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<T> GetAll()
        {
            return unitOfWork.Context.Set<T>().AsQueryable();
        }
        public void Add(T entity)
        {
            try
            {
                unitOfWork.Context.Set<T>().Add(entity);
            }
            catch (Exception e )
            {

                //throw e;
            }
           
        }

        public void AddRange(List<T> entities)
        {
            try
            {
                unitOfWork.Context.ChangeTracker.Clear();
                unitOfWork.Context.Set<T>().AddRange(entities);
            }
            catch (Exception e)
            {

                //throw e; 
            }

        }
        public void Update(T entity)
        {
            try
            {
                unitOfWork.Context.Attach(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {

                //throw e;
            }
      

        }

        public void UpdateRange(List<T> entities)
        {
            try
            {
                unitOfWork.Context.ChangeTracker.Clear();
                unitOfWork.Context.Set<T>().UpdateRange(entities);
            }
            catch (Exception e)
            {

                //throw e;
            }


        }


        public void Delete(T entity)
        {
            try
            {
                unitOfWork.Context.Entry<T>(entity).State = EntityState.Deleted;
            }
            catch (Exception e)
            {

               // throw e;
            }
            
        }

        public void DeleteRange(List<T> entities)
        {
            try
            {
                unitOfWork.Context.ChangeTracker.Clear();
                unitOfWork.Context.Set<T>().RemoveRange(entities);
            }
            catch (Exception e)
            {

               // throw e;
            }

        }





    }
}
