using Catalog.BLL.Services;
using Catalog.DAL.DataContext;
using Catalog.DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Text;
using MyCatalogSite.Middleware;
using MyCatalogSite.Filters;

namespace MySite
{
    public class Startup
    {
        private readonly Serilog.ILogger _serilogLogger;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _serilogLogger = new LoggerConfiguration()
                    .WriteTo.File(Configuration.GetValue<string>("LogFilePath"))
                    .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddLogging(builder => {
                builder.AddSerilog(_serilogLogger);
            });
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddDbContext<CatalogDataContext>(c =>
                   c.UseSqlServer(Configuration.GetConnectionString("CatalogDatabase")));
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<ICategoryRepository, CategoriesRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<LogActionFilter>();
            services.AddSwaggerDocument();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
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

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseRouting();
            app.UseAuthorization();

            app.UseWhen(context => context.Request.Path.ToString().StartsWith("/Categories/Image/", StringComparison.OrdinalIgnoreCase), appBuilder =>
            {
                appBuilder.UseImagesCache(options =>
                {
                    options.MaxImagesCount = 10;
                });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "images",
                pattern: "images/{id:int:min(1)}",
                defaults: new { controller = "Categories", action = "Image" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            WriteAdditionalInfoToLog();

            void WriteAdditionalInfoToLog()
            {
                _serilogLogger.Information($"Additional information: application location - {AppDomain.CurrentDomain.BaseDirectory.ToString()}");
                var sb = new StringBuilder();
                var configValues = Configuration.AsEnumerable();
                sb.AppendLine();
                foreach (var item in configValues)
                {
                    sb.AppendLine($"{item.Key}: {item.Value}");
                }
                
                _serilogLogger.Information($"Additional information: configValues - {sb.ToString()}");
            }
        }
    }
}
