using GithubStatistics.Application;
using GithubStatistics.WebAPI.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GithubStatistics.WebAPI
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;

        public Startup(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();

            if (_environment.IsDevelopment())
            {
                services.AddOpenApi();
            }

            services.AddCustomExceptionHandler();
            services.AddHealthChecks();

            services.AddControllers().AddNewtonsoftJson();
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
