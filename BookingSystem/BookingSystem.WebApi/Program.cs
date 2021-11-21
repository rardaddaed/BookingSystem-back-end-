using Autofac.Extensions.DependencyInjection;
using BookingSystem.Core.Constants;
using BookingSystem.Persistence;
using BookingSystem.Persistence.Seeders;
using BookingSystem.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

IConfiguration configuration = new ConfigurationBuilder()
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile("appsettings.json", false, true)
  .AddJsonFile(
    $"appsettings.{Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_VARIABLE_NAME) ?? Constants.DEFAULT_ENVIRONMENT_NAME}.json",
    true, true)
  .Build();

Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Debug()
  .ReadFrom.Configuration(configuration)
  .CreateLogger();

try
{
  Log.Information("WebApi Starting up");
  Log.Information($"Environment: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");
  var host = CreateHostBuilder(args).Build();

  using var scope = host.Services.CreateScope();
  var dbContext = scope.ServiceProvider.GetService<BSDbContext>();

  Log.Information("Migrating Booking System Database");
  dbContext.Database.Migrate();
  Log.Information("Migration Complete");

  await SeedData(dbContext);

  host.Run();
}
catch (Exception ex)
{
  Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
  Log.CloseAndFlush();
}

async Task SeedData(BSDbContext dbContext)
{
  Log.Information("Creating seed data for Booking System Database");
  var seeder = SeederFactory.Create(dbContext);
  await seeder.Seed();
  Log.Information("Seed data created");
}

IHostBuilder CreateHostBuilder(string[] args)
{
  return Host.CreateDefaultBuilder(args)
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
}