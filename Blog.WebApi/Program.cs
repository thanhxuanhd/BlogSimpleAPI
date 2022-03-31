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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices();


var app = builder.Build();

Configure();

SetupDatabase();
app.Run();

// This method gets called by the runtime. Use this method to add services to the container.
void ConfigureServices()
{
    builder.Services.AddDbContext<BlogDbContext>(options =>
                     options
                     .UseSqlServer(builder.Configuration
                     .GetConnectionString("BlogDbContext"), b => b.MigrationsAssembly("Blog.WebApi")))
                     .AddUnitOfWork<BlogDbContext>(); ;

    builder.Services.AddIdentity<User, UserRole>()
            .AddEntityFrameworkStores<BlogDbContext>()
            .AddDefaultTokenProviders();

    SetUpService(builder.Services);
}


// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
void Configure()
{
    if (app.Environment.IsDevelopment())
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

    // Authorization
    app.UseAuthentication();

    app.UseHttpsRedirection();
    app.UseCors(option =>
    {
        option.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });

    app.UseSwagger();

    // Enable middle ware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("../swagger/v1/swagger.json", "My API V1");
    });

    app.UseRouting();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    });
}

void SetUpService(IServiceCollection services)
{
    ///
    /// Config jwtOption Authentication Service
    ///
    var jwtAppSettingOptions = builder.Configuration.GetSection(nameof(JwtIssuerOptions));
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
    services.AddAuthentication
        (options =>
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
    services.AddAuthorization
        (
        options =>
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
    services.AddScoped<IRoleService, RoleService>();
    services.AddTransient<IJwtFactory, JwtFactory>();
    services.Configure<Configurations>(options => builder.Configuration.GetSection(nameof(Configurations)).Bind(options));

    // Auto Mapper Config For Asp.Net Core
    IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
    services.AddSingleton(mapper);
    //services.AddScoped<IMapper>(sp => new Mapper(mapper.ConfigurationProvider, sp.GetService));

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
    services.AddMvc().AddNewtonsoftJson(opts =>
    {
        // Force Camel Case to JSON
        opts.SerializerSettings.ContractResolver = new DefaultContractResolver();
    })
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

    services.AddControllers();

    services.AddApiVersioning(o =>
    {
        o.ReportApiVersions = true;
        o.AssumeDefaultVersionWhenUnspecified = true;
        o.DefaultApiVersion = new ApiVersion(1, 0);
    });

    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    });
}

void SetupDatabase()
{
    using (IServiceScope scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<BlogDbContext>();
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<UserRole>>();
            BlogDbInitializer.Initializer(context, userManager, roleManager);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating the database.");
        }
    }
}