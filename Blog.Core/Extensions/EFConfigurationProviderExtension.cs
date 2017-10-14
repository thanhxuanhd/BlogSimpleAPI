using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Extensions
{
    public static class EFConfigurationProviderExtension
    {
        public static IConfigurationBuilder AddEntityFrameworkConfig(this IConfigurationBuilder builder, Action<DbContextOptionsBuilder> setup)
        {
            return builder.Add(new EFConfigSource(setup));
        }
    }
}
