using BlogSimple.BlazorApp.Data.Interfaces;
using BlogSimple.BlazorApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogSimple.BlazorApp.PageModels;

[Authorize]
public class PostAddEditPage : ComponentBase
{
    [Parameter]
    public Guid? Id { get; set; }

    [BindProperty]
    public PostViewModel Model { get; set; } = new PostViewModel();

    [BindProperty]
    public List<PostCategoryViewModel> PostCategories { get; set; } = new List<PostCategoryViewModel>();

    [BindProperty]
    public EditContext editContext { get; set; }

    [BindProperty]
    public string errorMessage { get; set; }

    [Inject]
    private IPostService postService { get; set; }

    [Inject]
    private IPostCategoryService postCategoryService { get; set; }

    [Inject]
    private ILogger<PostPage> logger { get; set; }

    [Inject]
    private NavigationManager navigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            PostCategories = await postCategoryService.Get();

            if (Id.HasValue)
            {
                Model = await postService.Get(Id);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }
    }

    public async Task OnSaveChange()
    {
        errorMessage = "";
        try
        {
            if (Id.HasValue)
            {
                var isUpdate = await postService.Edit(Model);

                if (isUpdate)
                {
                    navigationManager.NavigateTo("/Posts");
                }
                else
                {
                    errorMessage = "Can not upate this . Please try again";
                }
            }
            else
            {
                var postId = await postService.Add(Model);

                if (postId != Guid.Empty)
                {
                    navigationManager.NavigateTo("/Posts");
                }
                else
                {
                    errorMessage = "Can not upate this . Please try again";
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }
    }
}