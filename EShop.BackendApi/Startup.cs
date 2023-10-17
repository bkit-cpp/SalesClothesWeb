
using EShop.Application.Catalog.CategoryTranslations;
using EShop.Application.Catalog.Products;
using EShop.Application.Interfaces;
using EShop.BackendApi.Extensions;
using EShop.Data.Entities;
using EShop.ViewModels.Systems.Users;
using EShop.Application.Authenticate;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using EShop.Application.Services.Users;
using EShop.Data.EF;
using EShop.Application.Services.Catalog.Products;
using EShop.Application.Common;
using EShop.Application.Services.Catalog.Categories;
using EShop.Application.Services.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EShop.BackendApi
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

            services.AddDbContext<EShopDbContext>(options =>
            options.UseSqlServer("Server=DESKTOP-28MP4Q4;Database=eShopSolution;Trusted_Connection=True;"));

            // Load app settings
            var appSettings = new AppSetting();
            appSettings.SecretKey = Configuration.GetSection("SecretKey").Value;
            appSettings.AllowedHosts = Configuration.GetSection("AllowedHosts").Value;

            // Register the app settings with the dependency injection container
            //AddSingleton - register only once time
            services.AddSingleton(appSettings);

            services.AddScoped<IEShopAuthentication, EShopAuthenticationService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IProductInCategoryService, ProductInCategoryService>();
            services.AddTransient<ISlideService, SlideService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IStorageService, FileStorageService>();
            services.AddTransient<ICategoryTranslationService, CategoryTranslationService>();
            services.AddTransient<IUserService, UserService>();
            services.AddControllers()
             .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger EShopSolution", Version = "v1" });
                //Custom token
                c.OperationFilter<SwaggerHeaderOperationFilter>("Token", "MyHeaderValue");
            });

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

                // For mobile apps, allow http traffic.
                app.UseHttpsRedirection();
            }

            app.UseMiddleware(typeof(GlobalErrorHandlingMiddleware));

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Eshop V1");
            });
        }
    }
}