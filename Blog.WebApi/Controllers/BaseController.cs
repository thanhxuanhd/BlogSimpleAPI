using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Blog.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Blog.WebApi.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class BaseController<T> : Controller
    {
        #region Properties

        public ILogger<T> Logger
        {
            get; private set;
        }

        public Guid CurrentUserId
        {
            get
            {
                var identity = (ClaimsPrincipal)HttpContext.User;
                IEnumerable<Claim> claims = identity.Claims;
                var accountId = claims.Where(c => c.Type.ToLower().Equals("id"))
                                   .Select(c => c.Value).SingleOrDefault();
                if (!string.IsNullOrEmpty(accountId))
                {
                    return new Guid(accountId);
                }
                return Guid.Empty;
            }
        }

        #endregion Properties

        #region Contructor

        public BaseController(ILogger<T> loggerFactory)
        {
            Logger = loggerFactory;
        }

        #endregion Contructor

        #region Protected-Methods

        protected virtual IActionResult DoActionWithReturnResult(Func<IActionResult> function)
        {
            IActionResult response = null;

            try
            {
                response = function.Invoke();
            }
            catch (BlogException ex)
            {
                Logger.LogError(ex.ToString());
                response = BadRequest(new string[] { ex.Message });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                response = StatusCode((int)HttpStatusCode.InternalServerError, "ERROR_SYSTEM");
            }

            return response;
        }

        protected JsonResult ResponseDataSuccess(object data)
        {
            return Json(data, new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });
        }

        #endregion Protected-Methods
    }
}