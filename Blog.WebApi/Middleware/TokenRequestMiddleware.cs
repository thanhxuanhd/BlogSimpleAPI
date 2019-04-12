using System;
using System.Linq;
using System.Threading.Tasks;
using Blog.WebApi.Auth;
using Blog.WebApi.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Blog.WebApi.Middleware
{
    public class TokenRequestMiddleware
    {
        #region Variables

        private readonly ILogger<TokenRequestMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly Configurations _options;
        private readonly IJwtFactory _jwtFactory;

        #endregion Variables

        #region Ctor

        public TokenRequestMiddleware(RequestDelegate next, IOptions<Configurations> options, ILogger<TokenRequestMiddleware> logger, IJwtFactory jwtFactory)
        {
            _next = next;
            _options = options.Value;
            _logger = logger;
            _jwtFactory = jwtFactory;
        }

        #endregion Ctor

        #region Action Request

        public Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"]
                 .FirstOrDefault(header => header.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase));

            if (token != null)
            {
                try
                {
                    var tokenValidation = token.Split("Bearer ")[1];
                    var result = _jwtFactory.ValidateTokenAsync(tokenValidation);
                    if (!result)
                    {
                        context.Response.StatusCode = 401;
                        return context.Response.WriteAsync("UnAuthoried");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                    _logger.LogInformation(ex.StackTrace);
                }
            }

            return this._next(context);
        }

        #endregion Action Request
    }

    public static class TokenRequestMiddlewareExtens
    {
        public static IApplicationBuilder UseSecurityMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenRequestMiddleware>();
        }
    }
}