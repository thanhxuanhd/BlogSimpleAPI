using AutoMapper;
using Blog.Core.Model;
using Blog.Service.Interface;
using Blog.Service.ViewModels;
using Blog.WebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Blog.WebApi.Controllers
{
    [Route("api/Account")]
    public class AccountController : BaseController<AccountController>
    {
        private IUserService _userService;
        private UserManager<User> _userManager;
        private RoleManager<UserRole> _roleManager;
        private ILogger<AccountController> _logger;
        private SignInManager<User> _signInManager;

        private readonly JsonSerializerSettings _serializerSettings;

        public AccountController(IUserService userService, UserManager<User> userManager,
            ILogger<AccountController> logger,
            RoleManager<UserRole> roleManager,
            SignInManager<User> signInManager) : base(logger)
        {
            _userService = userService;
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        [HttpPost("Create", Name = "CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(ModelState.ToString());
                return BadRequest(ModelState);
            }
            var user = Mapper.Map<UserViewModel, User>(model);
            user.UserName = model.Email;
            var userId = await _userManager.CreateAsync(user, model.Password);
            return Ok(userId);
        }

        [HttpGet(Name = "GetUser")]
        public IActionResult GetUser()
        {
            var users = _userService.GetList(0, 0);
            return Json(users);
        }

        [HttpGet("{Id}")]
        public IActionResult GetById(Guid Id)
        {
            return DoActionWithReturnResult(() =>
           {
               var users = _userService.GetById(Id);
               return Json(users);
           });
        }
    }
}