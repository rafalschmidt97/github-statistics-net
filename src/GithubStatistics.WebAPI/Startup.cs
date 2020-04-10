using GithubStatistics.Application;
using GithubStatistics.Infrastructure;
using GithubStatistics.WebAPI.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GithubStatistics.WebAPI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure();
            services.AddApplication();
            services.AddControllers();

            if (_environment.IsDevelopment())
            {
                services.AddOpenApi();
            }

            services.AddCustomExceptionHandler();
            services.AddHealthChecks();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
            {
                app.UseOpenApi();
            }

            app.UseCustomExceptionHandler();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });
        }
    }
}
