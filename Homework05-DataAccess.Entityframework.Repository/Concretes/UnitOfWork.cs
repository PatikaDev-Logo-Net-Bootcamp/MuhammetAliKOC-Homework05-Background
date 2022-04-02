using Homework05_DataAccess.Entityframework.Repository.Abstracts;
using Homework05_DataAccess.EntityFramework;

namespace Homework05_DataAccess.Entityframework.Repository.Concretes
{
    public class UnitOfWork : IUnitOfWork
    {
        public AppDbContext Context { get; }
        public UnitOfWork(AppDbContext context)
        {
            Context = context;
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
