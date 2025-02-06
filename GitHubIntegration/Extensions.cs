using GitHubIntegration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubIntegration
{
    public static class Extensions
    {
        public static void AddGitHubIntegration(this IServiceCollection services, Action<GitHubIntegrationOptions> ConfigurationOptions)
        {
            services.Configure(ConfigurationOptions);
            services.AddScoped<IGitHubService, GitHubService>();
        }
    }
}
