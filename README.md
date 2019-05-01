# Application

This application will be used by a sports coach to enter test results made by his athletes.

# Demo

Coming Soon

# Roadmap

Coming Soon

# Wiki

Coming Soon

# Technology

- C#
- ASP.NET Core 2.2
- JavaScript, jQuery
- Entity Framework Core
- MSSQL

# Prerequisites

- Visual Studio 2017
- .NET Core 2.2 (https://www.microsoft.com/net/core)
- Install Logging from below NuGet packages in Application.Web Project and Application.WebAPI Project one at a time.
 (Right click on Application.Web Project Go to manage NuGet packages and browse Below NuGet packages and install individually then same things will do on Application.WebAPI Project)
	- Serilog.Extensions.Logging
	- Serilog.Sinks.RollingFile
	- Serilog.Sinks.Seq

# How to run on local

- Open the Application.sln solution in Visual Studio
- Build the solution (default apps will be copied over to the "Apps" folder)
- Set the data provider of your choice in the appsettings.json file of Application.WebAPI project and modify the default connection string accordingly if needed.
- Run (F5 or Ctrl+F5)
- Database and seed data will be created automatically the first time you run the application.

# How to contribute

Coming Soon