using AutoMapper;
using Blog.Core;
using Blog.Core.Model;
using Blog.Infrastructure;
using Blog.Service.Interface;
using Blog.Service.Mapping;
using Blog.Service.Service;
using Blog.WebApi.Auth;
using Blog.WebApi.Helpers;
using Blog.WebApi.Middleware;
using Blog.WebApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Text;

namespace Blog.WebApi
{
    public class Startup
    {
        private const string SecretKey = "ja1XBRwlCBB3Xm68YIAK2A788YNrDoP9"; // todo: get this from somewhere secure

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BlogDbContext>(options =>
                     options
                     .UseSqlServer(Configuration
                     .GetConnectionString("BlogDbContext"), b => b.MigrationsAssembly("Blog.WebApi")))
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
                app.UseBrowserLink();
            }
            else
            {
                app.UseHttpsEnforcement();
            }

            //app.UseMiddleware<TokenRequestMiddleware>();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCors(option =>
            {
                option.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials();
            });

            app.UseSwagger();

            // Enable middle ware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "My API V1");
            });
            app.UseMvc();
        }

        public void SetUpService(IServiceCollection services)
        {
            ///
            /// Config jwtOption Authentication Service
            ///
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtAppSettingOptions[nameof(JwtIssuerOptions.SecretKey)]));
            services.Configure<JwtIssuerOptions>(options =>
                {
                    options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                    options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                    options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
                    options.SecretKey = jwtAppSettingOptions[nameof(JwtIssuerOptions.SecretKey)];
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
                    }).AddCookie();

            // API user claim policy
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
            services.AddScoped<ITokenService, TokenService>();
            services.AddTransient<IJwtFactory, JwtFactory>();
            services.Configure<Configurations>(options => Configuration.GetSection(nameof(Configurations)).Bind(options));

            // Auto Mapper Config For Asp.Net Core
            services.AddAutoMapper();

            Mapper.Initialize(cf =>
            {
                cf.AddProfile<DomainMappingToDtoProfile>();
            });
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));

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

            services.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1", new Info { Title = "Blog API", Version = "v1" });
                        });
        }
    }
}