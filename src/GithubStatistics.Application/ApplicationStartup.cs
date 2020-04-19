using System.Reflection;
using GithubStatistics.Application.Repositories.Infrastructure.Github;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GithubStatistics.Application
{
    public static class ApplicationStartup
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient<GithubFetcher>();
            services.AddHttpClient();

            services.Scan(scan =>
                scan.FromAssemblies(Assembly.GetExecutingAssembly())
                    .AddClasses()
                    .AsMatchingInterface()
                    .WithTransientLifetime());

            return services;
        }
    }
}
