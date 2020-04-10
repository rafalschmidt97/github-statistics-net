using GithubStatistics.WebAPI.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace GithubStatistics.WebAPI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseLogger()
                .UseStartup<Startup>();
    }
}
