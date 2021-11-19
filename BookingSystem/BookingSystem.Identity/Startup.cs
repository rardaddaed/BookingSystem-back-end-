using BookingSystem.Core.Constants;
using BookingSystem.Domain.Entities;
using BookingSystem.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookingSystem.Identity;

public class Startup
{
  private readonly IWebHostEnvironment _env;

  public Startup(IConfiguration configuration, IWebHostEnvironment env)
  {
    Configuration = configuration;
    _env = env;
  }

  public IConfiguration Configuration { get; }

  // This method gets called by the runtime. Use this method to add services to the container.
  public void ConfigureServices(IServiceCollection services)
  {
    services.AddDbContext<AppIdentityDbContext>(options =>
      options.UseSqlServer(Configuration.GetConnectionString(Constants.IDENTITY_DB_NAME),
          o => o.MigrationsAssembly(typeof(AppIdentityDbContext).Namespace))
        .ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning))
    );

    services.AddIdentity<AppUser, IdentityRole>()
      .AddEntityFrameworkStores<AppIdentityDbContext>()
      .AddDefaultTokenProviders();

    var identityServerBuilder = services.AddIdentityServer(options =>
    {
      options.Events.RaiseErrorEvents = true;
      options.Events.RaiseInformationEvents = true;
      options.Events.RaiseFailureEvents = true;
      options.Events.RaiseSuccessEvents = true;
    });

    if (!_env.IsProduction())
    {
      identityServerBuilder = identityServerBuilder.AddDeveloperSigningCredential();
    }
    else
    {
    }

    identityServerBuilder
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
        options.EnableTokenCleanup = true;
      });
  }

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    app.UseIdentityServer();
  }
}