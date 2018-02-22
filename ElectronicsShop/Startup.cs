﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ElectronicsShop.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ElectronicsShop
{
    public class Startup
    {
        //config of our project
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["Data:SportStoreProducts:ConnectionString"]));

            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(Configuration["Data:SportStoreIdentity:ConnectionString"]));
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();
            
            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddTransient<IOrderRepository, EFOrderRepository>();
            services.AddTransient<ICatalogRepository, EFCatalogRepository>();
            services.AddTransient<IProductStockRepository, EFProductStockRepository>();
            services.AddTransient<IAnalyticsCalculate, AnalyticsCalculate>();

            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //?????????????

            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();

            app.UseStaticFiles();
            app.UseSession();

            app.UseAuthentication();

            app.UseMvc(
                routes => {

                    routes.MapRoute(
                name: null,
                template: "{category}/Page{productPage:int}",
                defaults: new { controller = "Product", action = "List" }
                );

                    routes.MapRoute(
                name: null,
                template: "Page{productPage:int}",
                defaults: new
                {
                    controller = "Product",
                    action = "List",
                    productPage = 1
                });

                    routes.MapRoute(
                name: null,
                template: "{category}",
                defaults: new
                {
                    controller = "Product",
                    action = "List",
                    productPage = 1
                });


                    routes.MapRoute(
                name: null,
                template: "",
                defaults: new
                {
                    controller = "Product",
                    action = "List",
                    productPage = 1
                });

                    routes.MapRoute(name: "default", template: "{controller}/{action}/{id?}");
                });

            SeedData.EnsurePopulated(app);
            IdentitySeedData.EnsurePopulated(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Future functionality");
            });
        }
    }
}