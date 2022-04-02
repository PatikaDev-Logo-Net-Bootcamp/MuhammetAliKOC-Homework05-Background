﻿using Homework05_Business.DTO;
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
        IQueryable<User> GetAllUserAsQueryable();

        void AddUser(User user);
        void AddUsers(List<UserDTO> cameUsers);

        void UpdateUser(User user);
        void UpdateUsers(List<UserDTO> cameUsers);

        void DeleteUser(User user);
        void DeleteUsers(List<UserDTO> cameUsers);
    }
}
