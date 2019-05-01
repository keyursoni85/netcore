/*
Application - The Multi server multi operating system control panel © copyright 2018.

Application is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License 3.0 as published by the Free Software Foundation.

This file is part of the Application project and is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with this file; if not, please contact Application at https://www.Application.com/contact

https://www.gnu.org/licenses/lgpl-3.0.txt
*/


using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Application.Data.Configuration;

namespace Application.Data.Providers
{
    public class MSSQLDataProvider : IDataProvider
    {
        public DataProvider Provider { get; } = DataProvider.MSSQL;

        public IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationCPDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }

        public ApplicationCPDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationCPDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationCPDbContext(optionsBuilder.Options);
        }
    }
}