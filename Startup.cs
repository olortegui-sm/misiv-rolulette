using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyCaching.InMemory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RouletteMisiv.Infrastructure.Repositories;
using RouletteMisiv.Infrastructure.Services;

namespace RouletteMisiv
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Services HTTP API",
                    Version = Environment.GetEnvironmentVariable("VERSION"),
                    Description = "The Services Service HTTP API",
                });
            });

            services.AddControllers();

            services.AddScoped<IRouletteRepository, RouletteRepository>();
            services.AddScoped<IRouletteService, RouletteService>();

            services.AddEasyCaching(options =>
            {
                options.UseInMemory("default");
                options.UseInMemory(config =>
                {
                    config.DBConfig = new InMemoryCachingOptions
                    {
                        ExpirationScanFrequency = 300,
                        SizeLimit = 100,
                        EnableReadDeepClone = true,
                        EnableWriteDeepClone = false,
                    };
                    config.MaxRdSecond = 120;
                    config.EnableLogging = false;
                    config.LockMs = 5000;
                    config.SleepMs = 300;
                }, "default1");
            });

            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "misiv/{documentName}/swagger.json";
                });

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/misiv/v1/swagger.json", "My API V1");
                    c.RoutePrefix = "misiv";

                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
