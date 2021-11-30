using BookingSystem.Core.Constants;
using IdentityServer4.EntityFramework.DbContexts;
using System;
using BookingSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BookingSystem.Persistence.IdentitySeeders
{
  public class IdentitySeederFactory
  {
    public static BaseIdentitySeeder Create(ConfigurationDbContext configurationDbContext, string audience,
      RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
    {
      var env = Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_VARIABLE_NAME);

      BaseIdentitySeeder seeder = env switch
      {
        "Production" => new IdentitySeederProduction(configurationDbContext, audience, roleManager, userManager),
        "Development" => new IdentitySeederDevelopment(configurationDbContext, audience, roleManager, userManager),
        _ => new IdentitySeederLocal(configurationDbContext, audience, roleManager, userManager)
      };

      return seeder;
    }
  }
}