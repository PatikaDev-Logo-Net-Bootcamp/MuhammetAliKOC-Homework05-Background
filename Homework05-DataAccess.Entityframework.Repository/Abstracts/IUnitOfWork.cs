using Homework05_DataAccess.EntityFramework;
using System;

namespace Homework05_DataAccess.Entityframework.Repository.Abstracts
{
    public interface IUnitOfWork : IDisposable
    {
        AppDbContext Context { get; }
        void Commit();

    }
}
