using Blazored.LocalStorage;
using BlogSimple.BlazorApp.Data.Interfaces;
using BlogSimple.BlazorApp.Data.Services;
using BlogSimple.BlazorApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices();

var app = builder.Build();
Configure();

app.Run();

// This method gets called by the runtime. Use this method to add services to the container.
void ConfigureServices()
{
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    builder.Services.AddAuthorizationCore();
    builder.Services.AddBlazoredLocalStorage();

    builder.Services.Configure<APIConfiguration>(builder.Configuration.GetSection("APIConfiguration"));

    builder.Services.AddHttpClient<ITokenSevice, TokenService>();
    builder.Services.AddHttpClient<IPostCategoryService, PostCategoryService>();

    builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
    builder.Services.AddScoped<ITokenSevice, TokenService>();
    builder.Services.AddScoped<IPostCategoryService, PostCategoryService>();
    builder.Services.AddScoped<IPostService, PostService>();
}

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
void Configure()
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");
}