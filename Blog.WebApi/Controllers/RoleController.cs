using Blog.Service.Interface;
using Blog.Service.ViewModels;
using Blog.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.WebApi.Controllers
{
    [ApiVersion("1.0")]
    public class RoleController : BaseController<RoleController>
    {
        #region Variables

        private readonly IRoleService _roleService;

        #endregion

        #region Contructor

        public RoleController(IRoleService roleService, ILogger<RoleController> logger) : base(logger)
        {
            _roleService = roleService;
        }

        #endregion

        #region Actions

        [HttpGet]
        public IActionResult Get(string keyWord = "", string sortColunm = "", int pageIndex = 0, int pageSize = 15, bool desc = false)
        {
            return DoActionWithReturnResult(() =>
            {
                var roles = _roleService.GetList(pageIndex, pageSize, keyWord, sortColunm, desc);
                return ResponseDataSuccess(roles);
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            return DoActionWithReturnResult(() =>
            {
                var post = _roleService.GetById(id);
                return ResponseDataSuccess(post);
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] RoleViewModel model)
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

            return DoActionWithReturnResult(() =>
            {
                var roleId = _roleService.Add(model);
                _roleService.Save();
                return ResponseDataSuccess(roleId);
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] RoleViewModel model)
        {
            if (id == null)
            {
                var errors = new List<ValidationResponse>()
                {
                    new ValidationResponse()
                    {
                        Key = "Id",
                        Validations = new List<string>{ "ID_INVALID" }
                    }
                };
                return BadRequest(errors);
            }

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

            return DoActionWithReturnResult(() =>
            {
                var roleId = _roleService.Add(model);
                _roleService.Save();
                return ResponseDataSuccess(roleId);
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return DoActionWithReturnResult(() =>
            {
                bool isDelete = _roleService.Delete(id);
                if (isDelete)
                {
                    _roleService.Save();
                }
                return Json(isDelete);
            });
        }

        #endregion
    }
}