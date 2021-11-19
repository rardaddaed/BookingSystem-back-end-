using BookingSystem.Core.Constants;
using BookingSystem.Domain.Entities;
using BookingSystem.Persistence;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BookingSystem.Deploy
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      Log.Information($"Database: {Configuration.GetConnectionString(Constants.BS_DB_NAME)}");

      services.AddDbContext<BSDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString(Constants.BS_DB_NAME),
          o => o.MigrationsAssembly(typeof(BSDbContext).Namespace)));

      Log.Information($"Identity Database: {Configuration.GetConnectionString(Constants.IDENTITY_DB_NAME)}");

      services.AddDbContext<AppIdentityDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString(Constants.IDENTITY_DB_NAME),
          o => o.MigrationsAssembly(typeof(AppIdentityDbContext).Namespace)));

      services.AddIdentity<AppUser, IdentityRole>()
        .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddDefaultTokenProviders();

      services.AddIdentityServer()
        .AddDeveloperSigningCredential()
        .AddAspNetIdentity<AppUser>()
        .AddConfigurationStore(options =>
        {
          options.ConfigureDbContext = b => b.UseSqlServer(
            Configuration.GetConnectionString(Constants.IDENTITY_DB_NAME),
            o => o.MigrationsAssembly(typeof(AppIdentityDbContext).Namespace));
        })
        .AddOperationalStore(options =>
        {
          options.ConfigureDbContext = b => b.UseSqlServer(
            Configuration.GetConnectionString(Constants.IDENTITY_DB_NAME),
            o => o.MigrationsAssembly(typeof(AppIdentityDbContext).Namespace));
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
    }
  }
}