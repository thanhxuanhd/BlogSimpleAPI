using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Net;
using Blog.Core.Extensions;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
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
        #endregion

        #region Contructor
        public BaseController(ILogger<T> loggerFactory)
        {
            Logger = loggerFactory;
        }
        #endregion

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
        #endregion

    }
}
