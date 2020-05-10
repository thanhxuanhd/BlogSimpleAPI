using System;
using System.Collections.Generic;
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

        [BindProperty]
        public PostCategoryViewModel Model { get; set; } = new PostCategoryViewModel();

        [BindProperty]
        public List<PostCategoryViewModel> ParentPostCategories { get; set; } = new List<PostCategoryViewModel>();

        [BindProperty]
        public EditContext editContext { get; set; }

        [BindProperty]
        public string errorMessage { get; set; }

        [Inject]
        private IPostCategoryService postCategoryService { get; set; }

        [Inject]
        private ILogger<PostCategoryPage> logger { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ParentPostCategories = await postCategoryService.Get();

                if (Id.HasValue)
                {
                    Model = await postCategoryService.Get(Id);
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
                    var isUpdate = await postCategoryService.Edit(Model);

                    if (isUpdate)
                    {
                        navigationManager.NavigateTo("/PostCategory");
                    }
                    else
                    {
                        errorMessage = "Can not upate this category. Please try again";
                    }
                }
                else
                {
                    var postCategoryId = await postCategoryService.Add(Model);

                    if (postCategoryId != Guid.Empty)
                    {
                        navigationManager.NavigateTo("/PostCategory");
                    }
                    else
                    {
                        errorMessage = "Can not upate this category. Please try again";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}