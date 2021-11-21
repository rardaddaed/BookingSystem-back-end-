using BookingSystem.Core.Constants;
using System;

namespace BookingSystem.Persistence.Seeders
{
  public class SeederFactory
  {
    public static BaseSeeder Create(BSDbContext dbContext)
    {
      var env = Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_VARIABLE_NAME);

      BaseSeeder seeder = env switch
      {
        "Production" => new SeederProduction(dbContext),
        "Development" => new SeederDevelopment(dbContext),
        _ => new SeederLocal(dbContext)
      };

      return seeder;
    }
  }
}