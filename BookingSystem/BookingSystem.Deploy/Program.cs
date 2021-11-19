using BookingSystem.Core.Constants;
using BookingSystem.Deploy;
using BookingSystem.Persistence;
using BookingSystem.Persistence.Seeders;
using CommandLine;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;

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
Log.Information("Starting to migrate Booking System database...");

bool isSilent = false;

Parser.Default.ParseArguments<Options>(args)
  .WithParsed(o =>
  {
    isSilent = o.IsSilent;
  });

ConfirmToMigrate(isSilent);

try
{
  var host = CreateHostBuilder(args).Build();
  using var scope = host.Services.CreateScope();
  var dbContext = scope.ServiceProvider.GetService<BSDbContext>();
  var identityDbContext = scope.ServiceProvider.GetService<AppIdentityDbContext>();
  var persistedGrantDbContext = scope.ServiceProvider.GetService<PersistedGrantDbContext>();
  var configurationDbContext = scope.ServiceProvider.GetService<ConfigurationDbContext>();

  Log.Information("Migrating Booking System Database");
  dbContext.Database.Migrate();
  Log.Information("Migration Complete");

  Log.Information("Migrating Booking System Identity Database");
  identityDbContext.Database.Migrate();
  Log.Information("Migration Complete");

  Log.Information("Migrating Booking System PersistedGrant Database");
  persistedGrantDbContext.Database.Migrate();
  Log.Information("Migration Complete");

  Log.Information("Migrating Booking System Configuration Database");
  configurationDbContext.Database.Migrate();
  Log.Information("Migration Complete");

  await SeedData(dbContext, configurationDbContext);

  FinishMigration(isSilent);
}
catch (Exception ex)
{
  Log.Error(ex, "Migration error");
}

async Task SeedData(BSDbContext dbContext, ConfigurationDbContext configurationDbContext)
{
  Log.Information("Creating seed data for Booking System Database");
  var seeder = SeederFactory.Create(dbContext, configurationDbContext);
  await seeder.Seed();
  Log.Information("Seed data created");
}

void FinishMigration(bool isSilent)
{
  if (!isSilent)
  {
    Console.WriteLine();
    Console.WriteLine("Press enter to continue...");
    Console.ReadLine();
  }
}

void ConfirmToMigrate(bool isSilent)
{
  if (!isSilent)
  {
    Console.WriteLine();
    Console.WriteLine("Press 'y' to continue");
    var key = Console.ReadKey();

    if (key.KeyChar != 'y' && key.KeyChar != 'Y')
    {
      return;
    }

    Console.WriteLine();
  }
}

IHostBuilder CreateHostBuilder(string[] args) =>
  Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureWebHostDefaults(webBuilder =>
    {
      webBuilder
        .UseConfiguration(configuration)
        .UseContentRoot(Directory.GetCurrentDirectory())
        .UseStartup<Startup>();
    });

internal class Options
{
  [Option('s', HelpText = "Run migrations silently")]
  public bool IsSilent { get; set; }
}