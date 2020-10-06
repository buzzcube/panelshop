using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using PanelShop.Repository;
using PanelShop.Repos;

namespace PanelShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Replace "YourDbContext" with the name of your own DbContext derived class.
            services.AddDbContextPool<PanelShopRepoContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(
                        // Replace with your connection string.
                        //"server=localhost;user=root;password=1234;database=ef",
                        Configuration.GetConnectionString("mysqlConnection"),
                        // Replace with your server version and type.
                        mySqlOptions => mySqlOptions
                            .ServerVersion(new Version(8, 0, 21), ServerType.MySql)
                            .CharSetBehavior(CharSetBehavior.NeverAppend))
                    // Everything from this point on is optional but helps with debugging.
                    .UseLoggerFactory(
                        LoggerFactory.Create(
                            logging => logging
                                .AddConsole()
                                .AddFilter(level => level >= LogLevel.Information)))
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
            );

            services.AddControllers().AddNewtonsoftJson(s => {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Inject dependancy classes
            services.AddScoped<IPanelShopRepo, PanelShopRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
