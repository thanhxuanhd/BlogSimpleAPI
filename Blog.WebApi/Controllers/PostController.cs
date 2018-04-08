using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Blog.Service.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class PostController : Controller
    {
        #region Variables
        private readonly IPostService _postService;
        #endregion

        #region Ctor
        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        #endregion

        #region Action

        #endregion
    }
}
