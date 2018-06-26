using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JsonStringLocalizerTest.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc.Localization;

namespace JsonStringLocalizerTest
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();


            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en");
                // Formatting numbers, dates, etc.
                options.SupportedCultures = _supportedCultures;
                // UI strings that we have localized.
                options.SupportedUICultures = _supportedCultures;
            });

            services.AddMvc()
                .AddMvcLocalization()
                .AddViewLocalization()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.AddTransient<IStringLocalizer, JsonStringLocalizer>();

            // services.AddTransient(typeof(IHtmlLocalizer<>), typeof(HtmlLocalizerFix<>));
            // services.AddTransient<IHtmlLocalizer, HtmlLocalizerFix>();


        }

        CultureInfo[] _supportedCultures = new[]
                {
                    new CultureInfo("zh"),
                    new CultureInfo("en"),
                    new CultureInfo("fr"),
                };

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //app.UseRequestLocalization(new RequestLocalizationOptions
            //{
            //    DefaultRequestCulture = new RequestCulture("zh"),
            //    // Formatting numbers, dates, etc.
            //    SupportedCultures = supportedCultures,
            //    // UI strings that we have localized.
            //    SupportedUICultures = supportedCultures
            //});

            app.UseRequestLocalization();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
