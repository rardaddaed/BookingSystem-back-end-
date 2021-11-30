using BookingSystem.Domain.Entities;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BookingSystem.Persistence.IdentitySeeders
{
  public class IdentitySeederLocal : BaseIdentitySeeder
  {
    public IdentitySeederLocal(ConfigurationDbContext configurationDbContext, string audience,
      RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager) : base(configurationDbContext,
      audience, roleManager, userManager)
    {
    }
  }
}
