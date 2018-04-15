using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Blog.Service.Interface;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.WebApi.Controllers
{
    public class PostController : BaseController<PostController>
    {
        #region Variables
        private readonly IPostService _postService;
        ILogger<PostController> _logger;
        #endregion

        #region Ctor
        public PostController(IPostService postService, ILogger<PostController> logger) :base(logger)
        {
            _postService = postService;
            _logger = logger;
        }
        #endregion

        #region Action

        #endregion
    }
}
