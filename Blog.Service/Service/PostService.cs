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
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Post> _postRepository;
        private readonly IMapper _mapper;

        public PostService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _postRepository = _unitOfWork.GetRepository<Post>();
            _mapper = mapper;
        }

        public Guid Add(PostViewModel post, Guid currentUserId)
        {
            var entity = _mapper.Map<PostViewModel, Post>(post);

            entity.Id = Guid.NewGuid();
            entity.CreateBy = currentUserId;
            entity.CreateOn = DateTime.UtcNow;

            _postRepository.Insert(entity);

            return entity.Id;
        }

        public bool Delete(Guid id, Guid currentUserId)
        {
            var entity = _postRepository.FindBy(x => x.Id == id && !x.DeleteBy.HasValue).FirstOrDefault();

            if (entity == null)
            {
                return false;
            }
            entity.DeleteBy = currentUserId;
            entity.DeleteOn = DateTime.UtcNow;
            _postRepository.Update(entity);

            return true;
        }

        public PagingViewModel<PostViewModel> Get(string keyword, string sortColumn, Guid? postCategoryId, bool desc = false, int pageIndex = 0, int pageSize = 15)
        {
            var query = _postRepository.FindBy(x => !x.DeleteBy.HasValue);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Title.Contains(keyword));
            }

            if (postCategoryId.HasValue)
            {
                query = query.Where(x => x.PostCategoryId == postCategoryId.Value);
            }
            var totalCount = query.Count();
            query = desc ? query.OrderByDescending(x => x.Title) : query.OrderBy(x => x.Title);

            query = query.Skip(pageIndex * pageSize).Take(pageSize);

            var listPost = query.ProjectTo<PostViewModel>(_mapper.ConfigurationProvider)
                                .AsNoTracking()
                                .ToList();
            var pages = new PagingViewModel<PostViewModel>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Items = listPost,
                TotalCount = totalCount
            };

            return pages;
        }

        public PostViewModel GetById(Guid id)
        {
            var post = _postRepository.FindBy(x => x.Id == id).FirstOrDefault();

            if (post == null)
            {
                throw new BlogException("POST_NOT_FOUND");
            }

            return _mapper.Map<Post, PostViewModel>(post);
        }

        public void Save()
        {
            _unitOfWork.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }

        public bool Update(PostViewModel post, Guid currentUserId)
        {
            var entity = _postRepository.FindBy(x => x.Id == post.Id).FirstOrDefault();

            if (entity == null)
            {
                // throw new BlogException("POST_NOT_FOUND");
                return false;
            }

            var entityUpdate = _mapper.Map(post, entity);

            _postRepository.Update(entityUpdate);

            return true;
        }
    }
}