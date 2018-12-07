using Blog.Core.Model;
using Blog.WebApi.Auth;
using Blog.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.WebApi.Controllers
{
    public class TokenController : BaseController<TokenController>
    {
        #region Variables

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly ILogger<TokenController> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        #endregion

        #region Constructor

        public TokenController(UserManager<User> userManager,
                              ILogger<TokenController> logger, IJwtFactory jwtFactory,
                              IOptions<JwtIssuerOptions> jwtOptions,
                              RoleManager<UserRole> roleManager,
                              SignInManager<User> signInManager,
                              IConfiguration configuration,
                              ITokenService tokenService) : base(logger)
        {
            _userManager = userManager;
            _logger = logger;
            _jwtOptions = jwtOptions.Value;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        #endregion

        #region Action
        [AllowAnonymous]
        [HttpPost("Login", Name = "Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(ModelState.ToString());
                var errors = ModelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToList())
                          .Select(x => new ValidationResponse()
                          {
                              Key = x.Key,
                              Validations = x.Value
                          });
                return BadRequest(errors);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var errors = new List<ValidationResponse>()
                    {
                       new ValidationResponse
                       {
                           Key = "UserName",
                           Validations = new List<string>() { "USER_INVADLID" }
                       }
                    };
                return BadRequest(errors);
            }

            var claims = await BuildClaims(user);
            var token = _tokenService.GenerateAccessToken(claims);
            // Serialize and return the response
            var response = new AuthenResponseModel
            {
                Id = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub) != null ? claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).ToString() : string.Empty,
                AuthenToken = token,
                ExpiresIn = (int)_jwtOptions.ValidFor.TotalSeconds,
                Roles = claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList(),
                FullName = user.FullName,
                Email = user.Email
            };

            if (model.IncludeRefreshToken)
            {
                var refreshToken = GenerateRefreshToken();
                user.RefreshTokenHash = _userManager.PasswordHasher.HashPassword(user, refreshToken);
                response.RefreshTokenHash = user.RefreshTokenHash;
                await _userManager.UpdateAsync(user);
            }

            return new OkObjectResult(response);
        }

        [AllowAnonymous]
        [HttpPost("Refresh", Name = "Refresh")]
        public async Task<IActionResult> RefeshToken(RefreshTokenViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(ModelState.ToString());
                var errors = ModelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToList())
                          .Select(x => new ValidationResponse()
                          {
                              Key = x.Key,
                              Validations = x.Value
                          });
                return BadRequest(errors);
            }

            var principal = GetPrincipalFromExpiredToken(model.Token);
            if (principal == null)
            {
                var errors = new List<ValidationResponse>()
                    {
                       new ValidationResponse
                       {
                           Key = "Token",
                           Validations = new List<string>() { "TOKEN_NOT_INVALID" }
                       }
                    };
                return BadRequest(errors);
            }

            var user = await _userManager.GetUserAsync(principal);
            var verifyRefreshTokenResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.RefreshTokenHash, model.RefreshToken);
            if (verifyRefreshTokenResult == PasswordVerificationResult.Success)
            {
                var claims = await BuildClaims(user);
                var newToken = _tokenService.GenerateAccessToken(claims);
                return Ok(new { token = newToken });
            }

            return Forbid();
        }

        [HttpPost("Logout", Name = "LogoutAsync")]
        [AllowAnonymous]
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        #endregion

        #region Private Method

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidIssuer = _configuration["JwtIssuerOptions:Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtIssuerOptions:SecretKey"])),
                ValidateLifetime = false //in this case, we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !string.Equals(jwtSecurityToken.Header.Alg, SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }

        private async Task<IList<Claim>> BuildClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Expired, _configuration["JwtIssuerOptions:AccessTokenDurationInMinutes"])
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            return claims;
        }

        #endregion
    }
}