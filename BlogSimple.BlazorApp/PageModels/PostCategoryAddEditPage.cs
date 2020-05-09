using System;
using System.Threading.Tasks;
using BlogSimple.BlazorApp.Data.Interfaces;
using BlogSimple.BlazorApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlogSimple.BlazorApp.PageModels
{
    [Authorize]
    public class PostCategoryAddEditPage : ComponentBase
    {
        [Parameter]
        public Guid? Id { get; set; }

        [Inject]
        private IPostCategoryService postCategoryService { get; set; }

        [Inject]
        private ILogger<PostCategoryPage> logger { get; set; }

        [BindProperty]
        public PostCategoryViewModel Model { get; set; } = new PostCategoryViewModel();

        [BindProperty]
        public EditContext editContext { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (Id.HasValue)
                {
                    Model = await this.postCategoryService.Get(Id);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        public async Task OnSaveChange()
        {
            var acbe = this.Model;
        }
    }
}