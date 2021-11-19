using Autofac.Extensions.DependencyInjection;
using BookingSystem.Core.Constants;
using BookingSystem.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

IConfiguration configuration = new ConfigurationBuilder()
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
  .AddJsonFile(
    $"appsettings.{Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_VARIABLE_NAME) ?? Constants.DEFAULT_ENVIRONMENT_NAME}.json",
    optional: true, reloadOnChange: true)
  .Build();

Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Debug()
  .ReadFrom.Configuration(configuration)
  .CreateLogger();

try
{
  Log.Information("Starting up");
  Log.Information($"Environment: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");
  CreateHostBuilder(args).Build().Run();
}
catch (Exception ex)
{
  Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
  Log.CloseAndFlush();
}

IHostBuilder CreateHostBuilder(string[] args) =>
  Host.CreateDefaultBuilder(args)
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .UseSerilog()
    .ConfigureWebHostDefaults(webBuilder =>
    {
      webBuilder
        .UseKestrel()
        .UseConfiguration(configuration)
        .UseContentRoot(Directory.GetCurrentDirectory())
        .UseStartup<Startup>();
    });