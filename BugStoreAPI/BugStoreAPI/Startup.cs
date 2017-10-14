using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BugStoreAPI.Filters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using BugStoreDAL.EF;
using BugStoreDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using BugStoreDAL.Repositories;
using BugStoreDAL.EF.Initializers;

namespace BugStoreAPI
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();

            Configuration = builder.Build();
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_ => Configuration);

            services.AddMvcCore(config => config.Filters.Add(new BugStoreExceptionFilter(_env.IsDevelopment())))
                .AddJsonFormatters(j =>
                {
                    j.ContractResolver = new DefaultContractResolver();
                    j.Formatting = Formatting.Indented;
                });
            services.AddDbContext<StoreContext>(
                options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("BugStore"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure());
                });
            services.AddScoped<IProductRepositoty, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepositoty>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();

            if (env.IsDevelopment())
            {
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                    .CreateScope())
                {
                    StoreContext context = serviceScope.ServiceProvider.GetService<StoreContext>();
                    Initializer.InitializeData(context);
                }
            }
        }
    }
}
