using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace GithubStatistics.WebAPI.Extensions
{
    public static class LoggerExtension
    {
        public static IWebHostBuilder UseLogger(this IWebHostBuilder builder)
        {
            return builder.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration)
                .Enrich.FromLogContext());
        }
    }
}
