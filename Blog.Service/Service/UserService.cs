using AutoMapper;
using Blog.Core.Interface;
using Blog.Core.Model;
using Blog.Infrastructure;
using Blog.Service.Interface;
using Blog.Service.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Service.Service
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private UserManager<User> _userMaganer;

        public UserService(IUserRepository userRepository, UserManager<User> userMaganer)
        {
            //_unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _userMaganer = userMaganer;
        }


        public Guid Add(UserViewModel user)
        {
            var entity = Mapper.Map<UserViewModel, User>(user);
            if (IsDuplicateUser(entity))
            {
                throw new Exception("");
            }
            entity.Id = Guid.NewGuid();
            _userMaganer.CreateAsync(entity);
            return entity.Id;
        }

        public bool Delete(Guid userId)
        {
            throw new NotImplementedException();
        }

        public List<UserViewModel> GetList(int page, int pageSize, string keyWord = "", string sort = "", bool desc = false)
        {
            throw new NotImplementedException();
        }

        public bool Update(UserViewModel user)
        {
            throw new NotImplementedException();
        }

        private bool IsDuplicateUser(User user)
        {
            if (user.Id != null)
            {
                return _userRepository.FindBy(x => x.IsActive && x.UserName == user.UserName && x.Id != user.Id).Any();
            }
            else
            {
                return _userRepository.FindBy(x => x.IsActive && x.UserName == user.UserName).Any();
            }
        }
    }
}