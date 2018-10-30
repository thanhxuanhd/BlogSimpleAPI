using Blog.Service.Interface;
using Blog.Service.ViewModels;
using Blog.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.WebApi.Controllers
{
    public class RoleController : BaseController<RoleController>
    {
        private ILogger<RoleController> _logger;
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger) : base(logger)
        {
            _logger = logger;
            _roleService = roleService;
        }

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
                    _logger.LogInformation($"User Delete {CurrentUserId}");
                    _roleService.Save();
                }
                return Json(isDelete);
            });
        }
    }
}