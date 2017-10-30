using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Blog.Core.Model;
using Blog.Core;
using Microsoft.EntityFrameworkCore;
using Blog.Service.Interface;
using Blog.Service.Service;
using AutoMapper;
using Newtonsoft.Json.Serialization;
using Blog.Service.Mapping;
using Blog.WebApi.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Blog.WebApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Blog.Core.Repository;
using Blog.Core.Interface;
using Blog.WebApi.Auth;
using Blog.Infrastructure;

namespace Blog.WebApi
{
    public class Startup
    {
        private const string SecretKey = "ja1XBRwlCBB3Xm68YIAK2A788YNrDoP9"; // todo: get this from somewhere secure
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BlogDbContext>(options =>
                     options.UseSqlServer(Configuration.GetConnectionString("BlogDbContext"), b => b.MigrationsAssembly("Blog.WebApi")))
                     .AddUnitOfWork<BlogDbContext>(); ;

            services.AddIdentity<User, UserRole>()
                    .AddEntityFrameworkStores<BlogDbContext>()
                    .AddDefaultTokenProviders();

            services.AddScoped<IPostCategoryService, PostCategoryService>();

            SetUpService(services);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCors(option =>
            {
                option.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials();
            });
            app.UseMvc();
        }

        public void SetUpService(IServiceCollection services)
        {

            ///
            /// Config jwtOption Authen Service
            ///
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.TokenValidationParameters = tokenValidationParameters;
                options.SaveToken = true;
                options.IncludeErrorDetails = true;
            });

            //api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
            });

            //Repository
            //services.AddScoped<IUserRepository, UserRepository>();
            //Service
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostCategoryService, PostCategoryService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IJwtFactory, JwtFactory>();

            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainMappingToDtoProfile>();
            });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            // Add framework services.
            services.AddMvc().AddJsonOptions(opts =>
            {
                // Force Camel Case to JSON
                opts.SerializerSettings.ContractResolver = new DefaultContractResolver();
            }).AddViewLocalization()
                .AddDataAnnotationsLocalization();


        }
    }
}
