using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GithubStatistics.Application
{
    public static class ApplicationStartup
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
