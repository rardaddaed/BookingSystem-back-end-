using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingSystem.Domain.Entities;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BookingSystem.Persistence.IdentitySeeders
{
  public class IdentitySeederProduction : BaseIdentitySeeder
  {
    public IdentitySeederProduction(ConfigurationDbContext configurationDbContext, string audience,
      RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager) : base(configurationDbContext,
      audience, roleManager, userManager)
    {
    }
  }
}
