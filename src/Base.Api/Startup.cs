using AutoMapper;
using Base.Data;
using Base.Extensions;
using Base.Filters;
using Base.Repositories;
using Base.Repositories.Interfaces;
using Base.Services;
using Base.Services.Interfaces;
using Base.Settings;
using Base.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Text.Json.Serialization;

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

            services.AddAutoMapper(typeof(DummyVm));

            services.AddScoped<ExceptionFilter>();

            services.AddMvc(cfg => cfg.Filters.AddService<ExceptionFilter>())
                .AddJsonOptions(options =>
                {
                    var enumConverter = new JsonStringEnumConverter();
                    options.JsonSerializerOptions.Converters.Add(enumConverter);
                });

            var auth = Configuration.GetSection(nameof(Auth)).Get<Auth>();
            services.AddSingleton(auth);

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = auth.Authority;
                    options.Audience = auth.Audience;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier,
                        RoleClaimType = auth.RoleKey
                    };
                });

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

            //app.UseAuthentication();
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