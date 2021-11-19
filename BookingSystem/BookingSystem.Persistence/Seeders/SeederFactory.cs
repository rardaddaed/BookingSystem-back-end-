using BookingSystem.Core.Constants;
using System;
using IdentityServer4.EntityFramework.DbContexts;

namespace BookingSystem.Persistence.Seeders
{
  public class SeederFactory
  {
    public static BaseSeeder Create(BSDbContext dbContext, ConfigurationDbContext configurationDbContext)
    {
      var env = Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_VARIABLE_NAME);

      BaseSeeder seeder = env switch
      {
        "Production" => new SeederProduction(dbContext),
        _ => new SeederLocal(dbContext, configurationDbContext)
      };

      return seeder;
    }
  }
}