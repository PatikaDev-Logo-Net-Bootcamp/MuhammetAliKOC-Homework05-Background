using Homework05_Business.Abstracts;
using Homework05_DataAccess.Entityframework.Repository.Abstracts;
using Homework05_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework05_Business.Concretes
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> repository;
        private readonly IUnitOfWork unitOfWork;

        public UserService(IRepository<User> repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<User> GetAllUserAsQueryable()
        {
            return repository.GetAll();
        }


        public void AddUser(User user)
        {
            repository.Add(user);
            unitOfWork.Commit();
        }
        public void AddUsers(List<User> users)
        {
            foreach (var user in users)
            {
                repository.Add(user);
            }           
            unitOfWork.Commit();
        }

        public void UpdateUser(User user)
        {
            repository.Update(user);
            unitOfWork.Commit();
        }

        public void UpdateUsers(List<User> users)
        {
            foreach (var user in users)
            {
                repository.Update(user);
            }
            unitOfWork.Commit();
        }

        public void DeleteUser(User user)
        {
            repository.Delete(user);
            unitOfWork.Commit();
        }

        public void DeleteUsers(List<User> users)
        {
            foreach (var user in users)
            {
                repository.Delete(user);
            }
            unitOfWork.Commit();
        }
    }
}
