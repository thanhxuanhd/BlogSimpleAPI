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
using Microsoft.EntityFrameworkCore;

namespace Blog.Service.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRepository<UserRole> _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _roleRepository = unitOfWork.GetRepository<UserRole>();
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Guid Add(RoleViewModel role)
        {
            var entity = _mapper.Map<RoleViewModel, UserRole>(role);

            if (IsDuplicateUser(entity))
            {
                throw new BlogException("ROLE_DUPPLICATE");
            }

            entity.Id = Guid.NewGuid();

            _roleRepository.Insert(entity);

            return entity.Id;
        }

        public bool Delete(Guid Id)
        {
            var role = _roleRepository.Query()
                       .Include(x => x.AppUserRoles)
                       .Where(x => x.Id == Id).FirstOrDefault();

            if (role == null)
            {
                return false;
            }

            if (role.AppUserRoles.Count > 0)
            {
                return false;
            }

            _roleRepository.Delete(role);

            return true;
        }

        public RoleViewModel GetById(Guid id)
        {
            var entity = _roleRepository.FindBy(x => x.Id == id).FirstOrDefault();

            if (entity == null)
            {
                throw new BlogException("POST_NOT_FOUND");
            }

            return _mapper.Map<UserRole, RoleViewModel>(entity);
        }

        public PagingViewModel<RoleViewModel> GetList(int page, int pageSize, string keyWord = "", string sort = "", bool desc = false)
        {
            var query = _roleRepository.Query();

            if (!string.IsNullOrEmpty(keyWord))
            {
                query = query.Where(x => x.Name.Contains(keyWord));
            }

            var totalCount = query.Count();
            query = desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);

            var roles = query.Skip(page * pageSize).Take(pageSize)
                        .ProjectTo<RoleViewModel>(_mapper.ConfigurationProvider)
                        .AsNoTracking()
                        .ToList();

            var pages = new PagingViewModel<RoleViewModel>()
            {
                PageIndex = page,
                PageSize = pageSize,
                Items = roles,
                TotalCount = totalCount
            };

            return pages;
        }

        public void Save()
        {
            _unitOfWork.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }

        public bool Update(RoleViewModel role)
        {
            var entity = _roleRepository
                        .FindBy(x => x.Id == role.Id)
                        .FirstOrDefault();

            if (entity == null)
            {
                return false;
            }

            entity.Name = role.Name;
            entity.Description = entity.Description;
            entity.NormalizedName = role.Name.ToUpper();

            _roleRepository.Update(entity);

            return true;
        }

        private bool IsDuplicateUser(UserRole role)
        {
            if (role.Id != Guid.Empty)
            {
                return _roleRepository.FindBy(x => x.Name == role.Name).Any();
            }
            else
            {
                return _roleRepository.FindBy(x => x.Name == role.Name).Any();
            }
        }
    }
}