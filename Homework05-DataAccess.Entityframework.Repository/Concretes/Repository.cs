﻿using Homework05_DataAccess.Entityframework.Repository.Abstracts;
using Homework05_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework05_DataAccess.Entityframework.Repository.Concretes
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        public readonly IUnitOfWork unitOfWork;
        public Repository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public T Get(int id)
        {
            return unitOfWork.Context.Set<T>().Where(x => x.Id == id).FirstOrDefault();
        }

        public IQueryable<T> GetAll()
        {
            return unitOfWork.Context.Set<T>().AsQueryable();
        }
        public void Add(T entity)
        {
            unitOfWork.Context.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            unitOfWork.Context.Entry(entity).State = EntityState.Modified;
        }
        public void Delete(T entity)
        {
            unitOfWork.Context.Entry(entity).State = EntityState.Deleted;
        }
    }
}