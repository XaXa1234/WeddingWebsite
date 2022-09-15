using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Reflection;
using WeddingWebsite.Db;
using WeddingWebsite.Extensions;
using WeddingWebsite.Resources;
using WeddingWebsite.RouteModelConventions;
using WeddingWebsite.Services;

namespace WeddingWebsite
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
            services.AddMySql(Configuration);

            services.AddMyLocalization(Configuration);
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddScoped<IRsvpService, RsvpService>();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = false;
            });
            services.AddSingleton<CommonLocalizationService>();
            services.AddRazorPages(options =>options.Conventions.Add(new CultureTemplatePageRouteModelConvention()))
                                                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                                                    .AddDataAnnotationsLocalization(options =>
                                                    {
                                                        options.DataAnnotationLocalizerProvider = (type, factory) =>
                                                        {
                                                            var assemblyName = new AssemblyName(typeof(CommonResources).GetTypeInfo().Assembly.FullName);
                                                            return factory.Create(nameof(CommonResources), assemblyName.Name);
                                                        };
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseRouting();
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}