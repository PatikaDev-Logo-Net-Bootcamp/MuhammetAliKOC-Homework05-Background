using Homework05_DataAccess.Entityframework.Repository.Abstracts;
using Homework05_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework05_DataAccess.Entityframework.Repository.Concretes
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public readonly IUnitOfWork unitOfWork;
        public Repository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        //public T Get(int id)
        //{
        //    return unitOfWork.Context.Set<T>().Where(x => x.Id == id).FirstOrDefault();
        //}

        public IQueryable<T> GetAll()
        {
            return unitOfWork.Context.Set<T>().AsQueryable();
        }
        public void Add(T entity)
        {
            try
            {
               //unitOfWork.Context.Entry<T>(entity).State = EntityState.Detached;
                unitOfWork.Context.Set<T>().Add(entity);
                //unitOfWork.Context.Entry<T>(entity).State = EntityState.Added;
            }
            catch (Exception e )
            {

                throw e;
            }
           
        }

        public void AddRange(List<T> entities)
        {
            try
            {
                //unitOfWork.Context.Entry<T>(entity).State = EntityState.Detached;
                unitOfWork.Context.ChangeTracker.Clear();
                unitOfWork.Context.Set<T>().AddRange(entities);
                //unitOfWork.Context.ChangeTracker.DetectChanges();
                //unitOfWork.Context.Entry<T>(entity).State = EntityState.Added;
            }
            catch (Exception e)
            {

                throw e; 
            }

        }
        public void Update(T entity)
        {
            try
            {
                //unitOfWork.Context.Entry<T>(entity).State = EntityState.Modified;
                //unitOfWork.Context.Set<T>().Update(entity);
                unitOfWork.Context.Attach(entity).State = EntityState.Modified;

               // Attach

            }
            catch (Exception e)
            {

                throw e;
            }
      

        }

        public void UpdateRange(List<T> entities)
        {
            try
            {
                //unitOfWork.Context.Entry<T>(entity).State = EntityState.Modified;
                unitOfWork.Context.ChangeTracker.Clear();
                unitOfWork.Context.Set<T>().UpdateRange(entities);
                //unitOfWork.Context.Attach(entity).State = EntityState.Modified;

                // Attach

                //unitOfWork.Context.Entry<T>(entities).State = EntityState.Modified;

            }
            catch (Exception e)
            {

                throw e;
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

                throw e;
            }
            
        }

        public void DeleteRange(List<T> entities)
        {
            try
            {
                // unitOfWork.Context.Entry<T>(entity).State = EntityState.Deleted;
                unitOfWork.Context.ChangeTracker.Clear();
                unitOfWork.Context.Set<T>().RemoveRange(entities);
            }
            catch (Exception e)
            {

                throw e;
            }

        }





    }
}
