using BlogSimple.BlazorApp.Data.Interfaces;
using BlogSimple.BlazorApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogSimple.BlazorApp.PageModels;

[Authorize]
public class PostCategoryPage : ComponentBase
{
    [BindProperty]
    public PagingViewModel<PostCategoryViewModel> Models { get; set; } = new PagingViewModel<PostCategoryViewModel>()
    {
        Items = new List<PostCategoryViewModel>()
    };

    [BindProperty]
    public string Keyword { get; set; }

    [BindProperty]
    public string SortBy { get; set; }

    [Parameter]
    public int PageIndex { get; set; } = 1;

    [Parameter]
    public int PageSize { get; set; } = 20;

    [BindProperty]
    public int TotalPages => (int)Math.Ceiling(decimal.Divide(Models.TotalCount, PageSize));

    [BindProperty]
    public bool ShowPrevious => PageIndex > 1;

    [BindProperty]
    public bool ShowNext => PageIndex < TotalPages;

    [BindProperty]
    public bool ShowFirst => PageIndex != 1;

    [BindProperty]
    public bool ShowLast => PageIndex != TotalPages;

    [Inject]
    private IPostCategoryService postCategoryService { get; set; }

    [Inject]
    private ILogger<PostCategoryPage> logger { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await GetPostCategories();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }
    }

    public async Task PagerButtonClicked(int currentPage)
    {
        PageIndex = currentPage;
        await GetPostCategories();
    }

    private async Task GetPostCategories()
    {
        try
        {
            this.Models = await postCategoryService.Get(Keyword, SortBy, PageIndex - 1, PageSize);
        }
        catch (Exception ex)
        {
            this.Models = new PagingViewModel<PostCategoryViewModel>()
            {
                Items = new List<PostCategoryViewModel>()
            };

            logger.LogError(ex.Message);
        }
    }
}