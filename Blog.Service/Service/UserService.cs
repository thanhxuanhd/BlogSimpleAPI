using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Core.Extensions;
using Blog.Core.Model;
using Blog.Infrastructure;
using Blog.Service.Interface;
using Blog.Service.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Service.Service
{
    public class UserService : IUserService
    {
        private IUnitOfWork _unitOfWork;

        //private IUserRepository _userRepository;
        private IRepository<User> _userRepository;

        private UserManager<User> _userMaganer;

        public UserService(UserManager<User> userMaganer, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            //_userRepository = userRepository;
            _userRepository = _unitOfWork.GetRepository<User>();
            _userMaganer = userMaganer;
        }

        public Guid Add(UserViewModel user)
        {
            var entity = Mapper.Map<UserViewModel, User>(user);
            if (IsDuplicateUser(entity))
            {
                throw new BlogException("USER_DUPPLICATE");
            }
            entity.Id = Guid.NewGuid();
            _userMaganer.CreateAsync(entity);
            return entity.Id;
        }

        public bool Delete(Guid userId)
        {
            var entity = _userRepository
                 .FindBy(x => x.Id == userId)
                 .FirstOrDefault();
            if (entity == null)
            {
                return false;
            }

            entity.IsActive = false;

            _userRepository.Update(entity);

            return true;
        }

        public PagingViewModel<UserViewModel> GetList(int page, int pageSize, string keyWord = "", string sort = "", bool desc = false)
        {
            var query = _userRepository.FindBy(x => x.IsActive);

            if (!string.IsNullOrEmpty(keyWord))
            {
                query = query.Where(x => x.UserName.Contains(keyWord));
            }

            query = desc ? query.OrderByDescending(x => x.UserName) : query.OrderBy(x => x.UserName);

            var totalCount = query.Count();

            var users = query.Skip(page * pageSize).Take(pageSize)
                .ProjectTo<UserViewModel>().AsNoTracking().ToList();

            var pages = new PagingViewModel<UserViewModel>()
            {
                PageIndex = page,
                PageSize = pageSize,
                Items = users,
                TotalCount = totalCount
            };

            return pages;
        }

        public bool Update(UserUpdateViewModel user)
        {
            var entity = _userRepository.AllIncluding(x => x.AppUserRoles)
                .Where(x => x.Id == user.Id)
                .FirstOrDefault();

            if (entity == null)
            {
                return false;
            }

            entity.FullName = user.FullName;
            entity.PhoneNumber = user.PhoneNumber;
            entity.Sex = user.Sex;
            entity.BirthDay = user.BirthDay;

            _userRepository.Update(entity);
            return true;
        }

        public UserWidthRoleViewModel GetById(Guid Id)
        {
            var user = _userMaganer.FindByIdAsync(Id.ToString()).Result;
            if (user == null)
            {
                throw new BlogException("UserNotFound");
            }
            var roles = _userMaganer.GetRolesAsync(user).Result;
            return new UserWidthRoleViewModel()
            {
                BirthDay = user.BirthDay,
                FullName = user.FullName,
                Roles = roles.ToList(),
                Email = user.Email,
                Id = user.Id
            };
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

        public void Save()
        {
            _unitOfWork.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}