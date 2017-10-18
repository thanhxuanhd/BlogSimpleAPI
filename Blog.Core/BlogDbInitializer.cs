using Blog.Core.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Core
{
    public class BlogDbInitializer
    {
        public BlogDbInitializer()
        {
        }

        public static void Initializer(BlogDbContext context, UserManager<User> userManager)
        {

            if (!(context.PostCategorys.Count() > 0))
            {
                var userLogin = context.Users.FirstOrDefault(x => x.Email == "thanhxuanhd007@gmail.com");
                Guid userId;
                if (userLogin == null)
                {
                    var user = new User()
                    {
                        Id = Guid.NewGuid(),
                        UserName = "thanhxuanhd007@gmail.com",
                        EmailConfirmed = true,
                        BirthDay = new DateTime(1993, 9, 20),
                        FullName = "Nguyen Thanh Xuan",
                        IsActive = true,
                        Email = "thanhxuanhd007@gmail.com"
                    };
                    var userCreate = userManager.CreateAsync(user, "Abc@12345").Result;
                    var userDb = context.Users.FirstOrDefault(x => x.UserName == "thanhxuanhd007@gmail.com");
                    userId = userDb.Id;
                }
                else
                {
                    userId = userLogin.Id;
                }

                List<PostCategory> postCategorys = new List<PostCategory>();
                for (int i = 0; i < 5; i++)
                {
                    PostCategory postCategory = new PostCategory()
                    {
                        Id = Guid.NewGuid(),
                        CategoryName = $"Category {i}",
                        CagegoryDescription = $"Category {i}",
                        CreateOn = DateTime.Now,
                        CreateBy = userId
                    };

                    postCategorys.Add(postCategory);
                };
                context.PostCategorys.AddRange(postCategorys);

                List<Post> posts = new List<Post>();
                for (int i = 0; i < 5; i++)
                {
                    Post post = new Post()
                    {
                        Id = Guid.NewGuid(),
                        Title = $"Post {i}",
                        Content = "",
                        PostCategoryId = postCategorys.First().Id,
                        CreateOn = DateTime.Now,
                        CreateBy = userId
                    };

                    posts.Add(post);
                };

                context.Posts.AddRange(posts);

                context.SaveChanges();

            }
        }
    }
}