using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace ASP.NETCoreWebAPIDemo.Extension
{
    public static class SwaggerExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger(c =>
            {

            });
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP.NETCoreWebAPIDemo v1"));
        }

        public static void AddSwaggerConfig(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ASP.NET Core Web API Demo",
                    Version = "v1",
                    Description = "",
                });
            });
        }
    }
}
