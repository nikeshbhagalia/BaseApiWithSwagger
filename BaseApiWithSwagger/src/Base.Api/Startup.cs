﻿using AutoMapper;
using Base.Api.Data;
using Base.Api.Extensions;
using Base.Api.Repositories;
using Base.Api.Repositories.Interfaces;
using Base.Api.Services;
using Base.Api.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Base.Api
{
    public class Startup
    {
        private const string InMemoryDbIndicator = "InMemory";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddDbContext<Context>(SetDbContext);

            services.AddScoped<IDummyRepository, DummyRepository>();

            services.AddScoped<IDummyService, DummyService>();

            services.AddAutoMapper(typeof(Startup));

            services.AddMvc();

            services.AddCustomSwagger();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCustomSwagger();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<Context>();
                if (!context.Database.IsInMemory())
                {
                    context.Database.Migrate();
                }
            }
        }

        private void SetDbContext(DbContextOptionsBuilder options)
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");

            if (connection.Equals(InMemoryDbIndicator, StringComparison.OrdinalIgnoreCase))
            {
                options.UseInMemoryDatabase(InMemoryDbIndicator);
            }
            else
            {
                //Data Source=dummy.db
                options.UseSqlite(connection);
            }
        }
    }
}