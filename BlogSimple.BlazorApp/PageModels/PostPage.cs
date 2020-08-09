using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogSimple.BlazorApp.Data.Interfaces;
using BlogSimple.BlazorApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlogSimple.BlazorApp.PageModels
{
    [Authorize]
    public class PostPage : ComponentBase
    {
        [BindProperty]
        public PagingViewModel<PostViewModel> Models { get; set; } = new PagingViewModel<PostViewModel>()
        {
            Items = new List<PostViewModel>()
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
        private IPostService postService { get; set; }

        [Inject]
        private ILogger<PostPage> logger { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetPosts();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }


        public async Task PagerButtonClicked(int currentPage)
        {
            PageIndex = currentPage;
            await GetPosts();
        }

        private async Task GetPosts()
        {
            try
            {
                this.Models = await postService.Get(Keyword, SortBy, PageIndex - 1, PageSize);
            }
            catch (Exception ex)
            {
                this.Models = new PagingViewModel<PostViewModel>()
                {
                    Items = new List<PostViewModel>()
                };

                logger.LogError(ex.Message);
            }
        }
    }
}