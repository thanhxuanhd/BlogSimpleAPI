using Blog.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Blog.Service.ViewModels;
using Blog.Infrastructure;
using Blog.Core.Model;
using System.Linq;
using AutoMapper;

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
        public Guid Add(PostCategoryViewModel post)
        {
            throw new NotImplementedException();
        }

        public List<PostCategoryViewModel> Get(int pageIndex, int pageSize, string keyWord,string sortColumn)
        {
            var query = _postCategoryRepository.FindBy(x => !x.DeleteBy.HasValue);
            if (!string.IsNullOrEmpty(keyWord))
            {
                query = query.Where(x => x.CategoryName.Contains(keyWord));
            }

            var listpostCategorys = query.OrderBy(x=>x.CategoryName).Skip(pageIndex * pageSize).Take(pageSize).ToList();
            var listPostCategoryViewModel = Mapper.Map<List<PostCategory>, List<PostCategoryViewModel>>(listpostCategorys);
            return listPostCategoryViewModel;
        }

        public PostCategoryViewModel GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Update(PostCategoryViewModel post)
        {
            throw new NotImplementedException();
        }
    }
}
