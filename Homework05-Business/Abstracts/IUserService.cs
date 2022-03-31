using Homework05_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework05_Business.Abstracts
{
    public interface IUserService
    {
        User GetUser(int id);
        IQueryable<User> GetAllUserAsQueryable();

        void AddUser(User user);
        void AddUsers(List<User> users);

        void UpdateUser(User user);
        void UpdateUsers(List<User> users);

        void DeleteUser(User user);
        void DeleteUsers(List<User> users);
    }
}
