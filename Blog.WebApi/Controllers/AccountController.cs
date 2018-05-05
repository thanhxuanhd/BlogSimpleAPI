using AutoMapper;
using Blog.Core.Model;
using Blog.Service.Interface;
using Blog.Service.ViewModels;
using Blog.WebApi.Auth;
using Blog.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        private readonly IJwtFactory _jwtFactory;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly JwtIssuerOptions _jwtOptions;

        public AccountController(IUserService userService, UserManager<User> userManager,
            ILogger<AccountController> logger, IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtOptions,
            RoleManager<UserRole> roleManager,
             SignInManager<User> signInManager) : base(logger)
        {
            _userService = userService;
            _userManager = userManager;
            _logger = logger;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
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
        [AllowAnonymous]
        [HttpPost("Login", Name = "Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(ModelState.ToString());
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return BadRequest(
                    new ValidationResponse()
                    {
                        Key = "UserName",
                        Errors = new List<string>() { "USER_NOT_FOUND" }
                    });
            }
            var identity = await GetClaimsIdentity(model.UserName, model.Password);

            if (identity == null)
            {
                return BadRequest(new ValidationResponse()
                {
                    Key = "Password",
                    Errors = new List<string>() { "PASSWORD_IS_CORECT" }
                });
            }

            var role = await _userManager.GetRolesAsync(user);
            identity.AddClaim(new Claim("fullName", user.FullName));
            identity.AddClaim(new Claim("email", user.Email));
            var token = await _jwtFactory.GenerateEncodedToken(model.UserName, identity);
            // Serialize and return the response
            var response = new AuthenResponseModel
            {
                Id = identity.Claims.Single(c => c.Type == "id").Value,
                AuthenToken = token,
                ExpiresIn = (int)_jwtOptions.ValidFor.TotalSeconds,
                Roles = role.ToList(),
                FullName = identity.Claims.Single(c => c.Type == "fullName").Value
            };

            return new OkObjectResult(response);
        }

        [HttpPost("Logout", Name = "LogoutAsync")]
        [AllowAnonymous]
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return Ok();
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

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                // get the user to verifty
                var userToVerify = await _userManager.FindByNameAsync(userName);

                if (userToVerify != null)
                {
                    // check the credentials
                    if (await _userManager.CheckPasswordAsync(userToVerify, password))
                    {
                        return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id.ToString()));
                    }
                }
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

    }
}