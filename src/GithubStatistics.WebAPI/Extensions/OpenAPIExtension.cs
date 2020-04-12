using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace GithubStatistics.WebAPI.Extensions
{
    public static class OpenAPIExtension
    {
        public static void AddOpenApi(this IServiceCollection services)
        {
            services.AddSwaggerGen(configuration =>
            {
                configuration.SwaggerDoc("v1", CreateOpenApiInfo());
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                configuration.IncludeXmlComments(xmlPath);
            });
        }

        public static void UseOpenApi(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "GithubStatistics Open API");
            });
        }

        private static OpenApiInfo CreateOpenApiInfo()
        {
            var info = new OpenApiInfo
            {
                Title = "GithubStatistics",
                Description = "Recruitment task for Allegro",
            };

            return info;
        }
    }
}
