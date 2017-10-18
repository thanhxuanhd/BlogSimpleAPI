using Blog.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Blog.Service.ViewModels;
using Blog.Infrastructure;
using Blog.Core.Model;

namespace Blog.Service.Service
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Post> _postRepository;
        public PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _postRepository = _unitOfWork.GetRepository<Post>();
        }

        public Guid Add(PostViewModel post)
        {
            throw new NotImplementedException();
        }

        public List<PostViewModel> Get(int pageIndex, int pageSize, string sortColumn)
        {
            throw new NotImplementedException();
        }

        public PostViewModel GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Update(PostViewModel post)
        {
            throw new NotImplementedException();
        }
    }
}
