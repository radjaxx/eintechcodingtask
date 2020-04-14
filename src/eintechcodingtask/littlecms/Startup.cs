using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Common;
using core.Interfaces;
using littlecms.Repositories;
using littlecms.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace littlecms
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IApplicationConfiguration AppConfig { get; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            AppConfig = CreateAppConfiguration();
        } 

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddSingleton<IUnitOfWorkManager, UnitOfWorkManager>();
            services.AddSingleton<ICmsService, CmsService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private IApplicationConfiguration CreateAppConfiguration()
        {
            return new ApplicationConfiguration
            {
                DbConnectionString = Configuration.GetValue<string>("ConnectionStrings:littlecms")
            };
        }
    }
}
