using System.Threading.Tasks;
using BlogSimple.BlazorApp.Data.Interfaces;
using BlogSimple.BlazorApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace BlogSimple.BlazorApp.PageModels
{
    [Authorize]
    public class PageCategoryPage : ComponentBase
    {
        [BindProperty]
        public PagingViewModel<PostCategoryViewModel> Models { get; set; } = new PagingViewModel<PostCategoryViewModel>();

        [BindProperty]
        public string Keyword { get; set; }

        [BindProperty]
        public string SortBy { get; set; }

        [BindProperty]
        public int PageIndex { get; set; } = 0;

        [BindProperty]
        public int PageSize { get; set; } = 15;

        [Inject]
        private IPostCategoryService postCategoryService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Models = await postCategoryService.Get(Keyword, SortBy, PageIndex, PageSize);
        }
    }
}