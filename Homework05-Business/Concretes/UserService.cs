using Homework05_Business.Abstracts;
using Homework05_Business.DTO;
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
        public void AddUsers(List<UserDTO> cameUsers)
        {
            var users = GetAllUserAsQueryable().ToList();

            var leftJoinForAdd = from cameUser in cameUsers
                                 join user in users on cameUser.Id equals user.Id
                                 into total
                                 from userLeft in total.DefaultIfEmpty()
                                 select new
                                 {
                                     CameUser = cameUser,
                                     User = userLeft
                                 };

            //json içinde bulunan ancak veritabanında bulunmayan veriler. Bunları Veritabanına eklemeliyiz.
            var addusers = leftJoinForAdd
                            .Where(x => x.User == null)
                            .Select(x => new User()
                            {
                                Id = x.CameUser.Id,
                                UserId = x.CameUser.UserId,
                                Body = x.CameUser.Body,
                                Title = x.CameUser.Title
                            }
                            ).ToList();
     

            repository.AddRange(addusers);
            unitOfWork.Commit();
        }

        public void UpdateUser(User user)
        {
            repository.Update(user);
            unitOfWork.Commit();
        }

        public void UpdateUsers(List<UserDTO> cameUsers)
        {
            var users = GetAllUserAsQueryable().ToList();

            var joinForUpdate = from cameUser in cameUsers
                                join user in users on cameUser.Id equals user.Id
                                select new
                                {
                                    CameUser = cameUser,
                                    User = user
                                };

            //Hem json içinde hemde veritabanında bulunan veriler. Bunları Veritabanına güncellemeliyiz Jsondan nasıl geliyorlarsa.
            var updateusers = joinForUpdate
                            .Where(x => x.User != null && x.CameUser != null)
                            .Select(x => { x.User.UserId = x.CameUser.UserId; x.User.Title = x.CameUser.Title; x.User.Body = x.CameUser.Body; return x.User; })
                                    .ToList();

            repository.UpdateRange(updateusers);
            unitOfWork.Commit();
        }

        public void DeleteUser(User user)
        {
            repository.Delete(user);
            unitOfWork.Commit();
        }

        public void DeleteUsers(List<UserDTO> cameUsers)
        {
            var users = GetAllUserAsQueryable().ToList();

            var leftJoinForDelete = from user in users
                                    join cameUser in cameUsers on user.Id equals cameUser.Id
                                 into total
                                    from userLeft in total.DefaultIfEmpty()
                                    select new
                                    {
                                        CameUser = userLeft,
                                        User = user
                                    };


            //Json da olup, veritabanında olmayan veriler.
            var deleteusers = leftJoinForDelete
                            .Where(x => x.CameUser == null)
                            .Select(x => x.User).ToList();

            repository.DeleteRange(deleteusers);
            unitOfWork.Commit();
        }

 
    }
}
