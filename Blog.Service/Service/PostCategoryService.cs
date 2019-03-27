using System;
using System.Collections.Generic;
using System.Linq;
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
    public class PostCategoryService : IPostCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<PostCategory> _postCategoryRepository;

        public PostCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _postCategoryRepository = unitOfWork.GetRepository<PostCategory>();
        }

        public Guid Add(PostCategoryViewModel post, Guid currentUserId)
        {
            var entity = Mapper.Map<PostCategoryViewModel, PostCategory>(post);
            entity.Id = Guid.NewGuid();
            entity.CreateBy = currentUserId;
            entity.CreateOn = DateTime.UtcNow;
            _postCategoryRepository.Insert(entity);
            return entity.Id;
        }

        public bool Delete(Guid id, Guid currentUserId)
        {
            var entity = _postCategoryRepository.FindBy(x => x.Id == id && !x.DeleteBy.HasValue).FirstOrDefault();

            if (entity == null)
            {
                return false;
            }
            entity.DeleteBy = currentUserId;
            entity.DeleteOn = DateTime.UtcNow;
            _postCategoryRepository.Update(entity);

            return true;
        }

        public PagingViewModel<PostCategoryViewModel> Get(int pageIndex, int pageSize, string keyWord, string sortColumn)
        {
            var query = _postCategoryRepository.FindBy(x => !x.DeleteBy.HasValue);
            if (!string.IsNullOrEmpty(keyWord))
            {
                query = query.Where(x => x.CategoryName.Contains(keyWord));
            }
            var totalCount = query.Count();

            var listpostCategorys = query.OrderBy(x => x.CategoryName)
                                         .Skip(pageIndex * pageSize).Take(pageSize)
                                         .AsNoTracking()
                                         .ProjectTo<PostCategoryViewModel>()
                                         .ToList();
            var pages = new PagingViewModel<PostCategoryViewModel>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Items = listpostCategorys,
                TotalCount = totalCount
            };
            return pages;
        }

        public List<PostCategoryViewModel> GetAll()
        {
            var query = _postCategoryRepository.FindBy(x => !x.DeleteOn.HasValue)
                                               .OrderBy(x => x.CategoryName)
                                               .AsNoTracking()
                                               .ProjectTo<PostCategoryViewModel>()
                                               .ToList();
            return query;
        }

        public PostCategoryViewModel GetById(Guid id)
        {
            var entity = _postCategoryRepository.FindBy
                (x => x.Id == id && !x.DeleteBy.HasValue).FirstOrDefault();

            if (entity == null)
            {
                throw new BlogException("POST_CATEGORY_NOT_FOUND");
            }
            return Mapper.Map<PostCategory, PostCategoryViewModel>(entity);
        }

        public void Save()
        {
            _unitOfWork.SaveChanges();
        }

        public bool Update(PostCategoryViewModel postCategory, Guid currentUserId)
        {
            var entity = _postCategoryRepository.FindBy(x => x.Id == postCategory.Id && !x.DeleteBy.HasValue).FirstOrDefault();

            if (entity == null)
            {
                return false;
            }

            var entityUpdate = Mapper.Map(postCategory, entity);
            entity.ChangeBy = currentUserId;
            entity.ChangeOn = DateTime.UtcNow;
            _postCategoryRepository.Update(entityUpdate);

            return true;
        }

        public void UpdateParent(PostCategoryUpdateParentViewModel model, Guid currentUserId)
        {
            var entity = _postCategoryRepository.FindBy(x => x.Id == model.Id && !x.DeleteBy.HasValue).FirstOrDefault();

            if (entity == null)
            {
                throw new BlogException("POST_CATEGORY_NOT_FOUND");
            }

            entity.ParentId = model.ParentId;
            entity.ChangeBy = currentUserId;
            entity.ChangeOn = DateTime.UtcNow;

            _postCategoryRepository.Update(entity);
        }
    }
}