using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Core.Model;
using Blog.Service.Interface;
using Blog.Service.ViewModels;
using Blog.WebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Blog.WebApi.Controllers
{
    [ApiVersion("1.0")]
    public class AccountController : BaseController<AccountController>
    {
        #region Variable

        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        #endregion Variable

        #region Contructor

        public AccountController(IUserService userService, UserManager<User> userManager,
            ILogger<AccountController> logger,
            RoleManager<UserRole> roleManager,
            SignInManager<User> signInManager) : base(logger)
        {
            _userService = userService;
            _userManager = userManager;
        }

        #endregion Contructor

        #region Action

        [HttpPost("Create", Name = "CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToList())
                           .Select(x => new ValidationResponse()
                           {
                               Key = x.Key,
                               Validations = x.Value
                           });
                return BadRequest(errors);
            }
            var user = Mapper.Map<UserViewModel, User>(model);
            user.UserName = model.Email;
            var userId = await _userManager.CreateAsync(user, model.Password);
            return Ok(userId);
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody] UserUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToList())
                          .Select(x => new ValidationResponse()
                          {
                              Key = x.Key,
                              Validations = x.Value
                          });
                return BadRequest(errors);
            }
            var isUpdate = _userService.Update(model);

            if (!isUpdate)
            {
                return BadRequest();
            }
            return Ok(isUpdate);
        }

        [HttpGet(Name = "GetUser")]
        public IActionResult GetUser(int page = 0, int pageSize = 15, string keyWord = "", string sort = "", bool desc = false)
        {
            var users = _userService.GetList(page, pageSize, keyWord, sort, desc);
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

        #endregion Action
    }
}