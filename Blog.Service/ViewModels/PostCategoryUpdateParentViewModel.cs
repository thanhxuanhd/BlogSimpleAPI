using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blog.Service.ViewModels
{
    public class PostCategoryUpdateParentViewModel
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }
    }
}
