using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Core.Extensions;
using Blog.Core.Model;
using Blog.Infrastructure;
using Blog.Service.Interface;
using Blog.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var entity = Mapper.Map<PostViewModel, Post>(post);

            entity.Id = Guid.NewGuid();

            _postRepository.Insert(entity);

            return entity.Id;
        }

        public List<PostViewModel> Get(string keyword, bool desc = false, int pageIndex = 0, int pageSize = 15)
        {
            var query= _postRepository.FindBy(x => !x.DeleteBy.HasValue);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Title.Contains(keyword));
            }

            query = desc ? query.OrderByDescending(x=>x.Title) : query.OrderBy(x=>x.Title);

            query = query.Skip(pageSize - 1).Take(pageSize);

            return query.ProjectTo<PostViewModel>().ToList();
        }

        public PostViewModel GetById(Guid id)
        {
            var post = _postRepository.FindBy(x => x.Id == id).FirstOrDefault();

            if (post == null)
            {
                throw new BlogException("POST_NOT_FOUND");
            }

            return Mapper.Map<Post, PostViewModel>(post);
        }

        public void Save()
        {
            _unitOfWork.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }

        public void Update(PostViewModel post)
        {
            var entity = _postRepository.FindBy(x => x.Id == post.Id).FirstOrDefault();

            if (entity == null)
            {
                throw new BlogException("POST_NOT_FOUND");
            }

           var entityUpdate = Mapper.Map<PostViewModel, Post>(post);

            _postRepository.Update(entityUpdate);
        }
    }
}