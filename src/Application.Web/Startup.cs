using Application.Data;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Globalization;
using System.Reflection;

namespace Application.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel
                .Information().
                WriteTo.RollingFile(Configuration["ErrorLoggingPath"] + "Log-{Date}.text", Serilog.Events.LogEventLevel.Information, retainedFileCountLimit: 4)
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationCPDbContext>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                //options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc();


            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //        options.LoginPath = "/Home/Login";
            //        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            //    })
            //.AddCookie("Temp");

            //services.AddSingleton<LocService>();
            //services.AddLocalization(options => options.ResourcesPath = "Resources");
            //services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, opts => { opts.ResourcesPath = "Resources"; }).AddDataAnnotationsLocalization(
            //   options =>
            //   {
            //       options.DataAnnotationLocalizerProvider = (type, factory) =>
            //       {
            //           var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
            //           return factory.Create("SharedResource", assemblyName.Name);
            //       };
            //   });

            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    var supportedCultures = new[]
            //    {
            //        new CultureInfo("en-US"),
            //        new CultureInfo("de-DE")
            //    };

            //    options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
            //    options.SupportedCultures = supportedCultures;
            //    options.SupportedUICultures = supportedCultures;
            //    options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
            //});

            services.AddScoped<Application.Data.IUserRepository, Application.Data.Domain.UserRepository>();

            //services.AddMvc(option =>
            //{
            //    option.Filters.Add(new RequireHttpsAttribute());
            //    option.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            //});

            //services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromMinutes(20);
            //    options.Cookie.HttpOnly = true;
            //});

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy(
            //        "ManageStore",
            //        authBuilder =>
            //        {
            //            authBuilder.RequireClaim("ManageStore", "Allowed");
            //        });
            //});

            services.AddOptions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IAntiforgery antiForgery)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

    //            routes.MapRoute(
    //"404-PageNotFound",
    //"{*url}",
    //new { controller = "StaticContent", action = "PageNotFound" }
    //);
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute("404-PageNotFound","{*url}",
                    new { controller = "Account", action = "Error" }
                    );
            });
        }
    }
}