﻿using Blog.Service.Interface;
using Blog.Service.Validations;
using Blog.Service.ViewModels;
using Blog.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Blog.WebApi.Controllers;

[ApiVersion("1.0")]
public class PostCategoryController : BaseController<PostCategoryController>
{
    #region Variables

    private readonly IPostCategoryService _postCategoryService;

    #endregion Variables

    #region Constructor

    public PostCategoryController(ILogger<PostCategoryController> logger, IPostCategoryService postCategoryService) : base(logger)
    {
        _postCategoryService = postCategoryService;
    }

    #endregion Constructor

    #region Action

    // GET: api/values
    [HttpGet]
    public IActionResult Get(string keyWord = "", string sortColunm = "", int pageIndex = 0, int pageSize = 15)
    {
        return DoActionWithReturnResult(() =>
        {
            var listPostCategory = _postCategoryService.Get(pageIndex, pageSize, keyWord, sortColunm);
            return ResponseDataSuccess(listPostCategory);
        });
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        return DoActionWithReturnResult(() =>
        {
            var postCategory = _postCategoryService.GetById(id);
            return ResponseDataSuccess(postCategory);
        });
    }

    [HttpPost]
    public IActionResult Post([FromBody] PostCategoryViewModel model)
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
            var postId = _postCategoryService.Add(model, CurrentUserId);
            _postCategoryService.Save();
            return Json(postId);
        });
    }

    [HttpPut]
    public IActionResult Update([FromBody] PostCategoryViewModel model)
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
            bool isUpdate = _postCategoryService.Update(model, CurrentUserId);
            if (isUpdate)
            {
                _postCategoryService.Save();
            }
            return Json(isUpdate);
        });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        return DoActionWithReturnResult(() =>
        {
            bool isDelete = _postCategoryService.Delete(id, CurrentUserId);
            if (isDelete)
            {
                _postCategoryService.Save();
            }
            return Json(isDelete);
        });
    }

    [HttpPut("UpdateParent", Name = "UpdateParent")]
    public IActionResult UpdateParent([FromBody] PostCategoryUpdateParentViewModel model)
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

        var validator = new PostCategoryUpdateParentValations();
        var result = validator.Validate(model);
        if (!result.IsValid)
        {
            var errors = result.Errors.GroupBy(x => x.PropertyName)
                       .Select(x => new ValidationResponse()
                       {
                           Key = x.Key,
                           Validations = x.ToList().Select(k => k.ErrorMessage).ToList()
                       }).ToList();
            return BadRequest(errors);
        }

        return DoActionWithReturnResult(() =>
        {
            _postCategoryService.UpdateParent(model, CurrentUserId);
            _postCategoryService.Save();
            return Ok();
        });
    }

    [HttpGet("GetAll")]
    public IActionResult GetAll()
    {
        return DoActionWithReturnResult(() =>
        {
            var listPostCategory = _postCategoryService.GetAll();
            return ResponseDataSuccess(listPostCategory);
        });
    }

    #endregion Action
}