using System;
/*
Application - The Multi server multi operating system control panel © copyright 2018.

Application is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License 3.0 as published by the Free Software Foundation.

This file is part of the Application project and is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with this file; if not, please contact Application at https://www.Application.com/contact

https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using Elmah.Io.AspNetCore;
using Application.Data;
using Application.Framework.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Application.WebAPI
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
            string DefaultConnection = Configuration["Data:Provider"].ToSafeString();

            if (DefaultConnection != "MSSQL")
            {
                services.AddDbContext<ApplicationCPDbContext>(options =>
              options.UseMySQL(Configuration.GetConnectionString("MySqlConnectionString")));
            }
            else
            {
                services.AddDbContext<ApplicationCPDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("SqlConnectionString")));
            }

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.ExpireTimeSpan = TimeSpan.FromDays(7);
               }
               );

            //services.Configure<LdapConfig>(Configuration.GetSection("ldap"));

            services.AddScoped<Application.Data.IUserRepository, Application.Data.Domain.UserRepository>();
            //services.AddScoped<Application.Data.IPermissionsRepository, Application.Data.Domain.PermissionsRepository>();
            //services.AddScoped<Application.Data.IPluginsRepository, Application.Data.Domain.PluginsRepository>();
            //services.AddScoped<Application.Data.IExternalLoginRepository, Application.Data.Domain.ExternalLoginRepository>();
            //services.AddScoped<Application.Data.ISystemSettingsRepository, Application.Data.Domain.SystemSettingsRepository>();
            //services.AddScoped<Application.Data.IMenuRepository, Application.Data.Domain.MenuRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}
