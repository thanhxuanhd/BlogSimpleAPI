using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Blog.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Blog.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Blog.Core.Model;

namespace Blog.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            using (var scope = host.Services.CreateScope())
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

            host.Run();
        }

        private static void SetupConfiguration(WebHostBuilderContext hostingContext, IConfigurationBuilder configBuilder)
        {
            configBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            configBuilder.AddEnvironmentVariables();

            var configuration = configBuilder.Build();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration(SetupConfiguration)
                .Build();
    }
}
