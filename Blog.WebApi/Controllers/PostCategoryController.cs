using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Blog.Service.Interface;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class PostCategoryController : BaseController<PostCategoryController>
    {
        private ILogger<PostCategoryController> _logger;
        private readonly IPostCategoryService _postCategoryService;
        public PostCategoryController(ILogger<PostCategoryController> logger, IPostCategoryService postCategoryService) : base(logger)
        {
            _postCategoryService = postCategoryService;
            _logger = logger;
        }
        // GET: api/values
        [HttpGet]
        public IActionResult Get(int pageIndex,int pageSize, string keyWord= "",string sortColunm="")
        {
            return DoActionWithReturnResult(()=> {
                var listPostCategory = _postCategoryService.Get(pageIndex, pageSize, keyWord, sortColunm);
                return ResponseDataSuccess(listPostCategory);
            });
        }
    }
}
