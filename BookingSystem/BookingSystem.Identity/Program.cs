using BookingSystem.Core.Constants;
using BookingSystem.Domain.Entities;
using BookingSystem.Identity;
using BookingSystem.Persistence;
using BookingSystem.Persistence.IdentitySeeders;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
  var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
  Log.Information("Identity Starting up");
  Log.Information($"Environment: {env}");
  var host = CreateHostBuilder(args).Build();

  using var scope = host.Services.CreateScope();
  var identityDbContext = scope.ServiceProvider.GetService<AppIdentityDbContext>();
  var persistedGrantDbContext = scope.ServiceProvider.GetService<PersistedGrantDbContext>();
  var configurationDbContext = scope.ServiceProvider.GetService<ConfigurationDbContext>();

  Log.Information("Migrating Booking System Identity Database");
  identityDbContext.Database.Migrate();
  Log.Information("Migration Complete");

  Log.Information("Migrating Booking System PersistedGrant Database");
  persistedGrantDbContext.Database.Migrate();
  Log.Information("Migration Complete");

  Log.Information("Migrating Booking System Configuration Database");
  configurationDbContext.Database.Migrate();
  Log.Information("Migration Complete");

  await SeedData(configurationDbContext, scope);

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

IHostBuilder CreateHostBuilder(string[] args)
{
  return Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseConfiguration(configuration).UseStartup<Startup>(); });
}

async Task SeedData(ConfigurationDbContext configurationDbContext, IServiceScope scope)
{
  Log.Information("Creating identity seed data for Booking System Database");
  var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
  var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
  var audience = configuration.GetValue<string>(Constants.AUTHORIZATION_AUDIENCE);
  var seeder = IdentitySeederFactory.Create(configurationDbContext, audience, roleManager, userManager);
  await seeder.Seed();
  Log.Information("Identity seed data created");
}