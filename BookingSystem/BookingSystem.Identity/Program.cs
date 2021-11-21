using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BookingSystem.Core.Constants;
using BookingSystem.Identity;
using BookingSystem.Persistence;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

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

  await SeedData(configurationDbContext, env);

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
    .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}

async Task SeedData(ConfigurationDbContext configurationDbContext, string env)
{
  Log.Information("Creating seed data for Booking System Database");
  switch (env)
  {
    case "Production":
      await SeedIdentityProduction(configurationDbContext);
      break;
    case "Development":
      await SeedIdentityDevelopment(configurationDbContext);
      break;
    default:
      await SeedIdentityLocal(configurationDbContext);
      break;
  }

  await SeedIdentityLocal(configurationDbContext);
  Log.Information("Seed data created");
}

#region seed identity

async Task SeedIdentityLocal(ConfigurationDbContext configurationDbContext)
{
  #region Initialise

  var clients = new List<Client>
  {
    new()
    {
      ClientId = "demouser",
      ClientSecrets = new List<Secret> { new("1".ToSha256()) },
      AllowedGrantTypes = new List<string> { GrantType.ClientCredentials },
      AllowedScopes = { "write", "read", "openid", "profile", "email" },
      AccessTokenLifetime = 86400,
      AccessTokenType = AccessTokenType.Jwt
    }
  };

  var apiResources = new List<ApiResource>
  {
    new()
    {
      Name = "identity-booking-system-api",
      DisplayName = "Identity Booking System API",
      Scopes = new List<string>
      {
        "write",
        "read"
      }
    }
  };

  IEnumerable<ApiScope> apiScopes = new List<ApiScope>
  {
    new("openid"),
    new("profile"),
    new("email"),
    new("read"),
    new("write")
  };

  #endregion Initialise

  foreach (var client in clients)
  {
    var item = await configurationDbContext.Clients.FirstOrDefaultAsync(c => c.ClientId == client.ClientId);

    if (item == null)
    {
      await configurationDbContext.Clients.AddAsync(client.ToEntity());
    }
    else
    {
      var clientEntity = client.ToEntity();

      item.ClientId = clientEntity.ClientId;
      item.AccessTokenType = clientEntity.AccessTokenType;
      item.AccessTokenLifetime = clientEntity.AccessTokenLifetime;
    }
  }

  foreach (var resource in apiResources)
  {
    var item = await configurationDbContext.ApiResources.FirstOrDefaultAsync(c => c.Name == resource.Name);

    if (item == null)
    {
      await configurationDbContext.ApiResources.AddAsync(resource.ToEntity());
    }
    else
    {
      var resourceEntity = resource.ToEntity();

      item.Name = resourceEntity.Name;
      item.DisplayName = resourceEntity.DisplayName;
      item.Scopes = resourceEntity.Scopes;
    }
  }

  foreach (var scope in apiScopes)
  {
    var item = await configurationDbContext.ApiScopes.FirstOrDefaultAsync(c => c.Name == scope.Name);

    if (item == null)
    {
      await configurationDbContext.ApiScopes.AddAsync(scope.ToEntity());
    }
    else
    {
      var scopeEntity = scope.ToEntity();

      item.Name = scopeEntity.Name;
    }
  }

  await configurationDbContext.SaveChangesAsync();
}

async Task SeedIdentityProduction(ConfigurationDbContext configurationDbContext)
{
}

async Task SeedIdentityDevelopment(ConfigurationDbContext configurationDbContext)
{
  await SeedIdentityLocal(configurationDbContext);
}

#endregion