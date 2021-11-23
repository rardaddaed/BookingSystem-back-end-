using BookingSystem.Domain.Entities;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookingSystem.Persistence.IdentitySeeders;

public abstract class BaseIdentitySeeder
{
  protected readonly ConfigurationDbContext _configurationDbContext;
  protected readonly string _audience;
  protected readonly RoleManager<IdentityRole> _roleManager;
  protected readonly UserManager<AppUser> _userManager;

  protected BaseIdentitySeeder(ConfigurationDbContext configurationDbContext, string audience,
    RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
  {
    _configurationDbContext = configurationDbContext;
    _audience = audience;
    _roleManager = roleManager;
    _userManager = userManager;
  }

  public virtual async Task Seed()
  {
    await SeedClients();
    await SeedApiResources();
    await SeedApiScopes();
    await SeedRolesAndUsers();
  }

  protected virtual async Task SeedRolesAndUsers()
  {
    var userRole = await _roleManager.FindByNameAsync("user");
    if (userRole == null)
    {
      userRole = new IdentityRole
      {
        Name = "user"
      };
      await _roleManager.CreateAsync(userRole);
    }

    var bookingAdminRole = await _roleManager.FindByNameAsync("bookingadmin");
    if (bookingAdminRole == null)
    {
      bookingAdminRole = new IdentityRole
      {
        Name = "bookingadmin"
      };
      await _roleManager.CreateAsync(bookingAdminRole);
    }

    var configAdminRole = await _roleManager.FindByNameAsync("configadmin");
    if (configAdminRole == null)
    {
      configAdminRole = new IdentityRole
      {
        Name = "configadmin"
      };
      await _roleManager.CreateAsync(configAdminRole);
    }

    var demouser = await _userManager.FindByNameAsync("demouser");
    if (demouser == null)
    {
      demouser = new AppUser
      {
        UserName = "demouser",
        Email = "demouser@bookingsystem.com",
        EmailConfirmed = true
      };
      var result = await _userManager.CreateAsync(demouser, "Pa$$w0rd");
      if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

      result = await _userManager.AddClaimsAsync(demouser, new[]
      {
        new Claim(JwtClaimTypes.Name, "Demo User"),
        new Claim(JwtClaimTypes.GivenName, "Demo"),
        new Claim(JwtClaimTypes.FamilyName, "User"),
        new Claim(JwtClaimTypes.Email, "bookingadmin@bookingsystem.com")
      });
      if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

      if (!await _userManager.IsInRoleAsync(demouser, userRole.Name))
        await _userManager.AddToRoleAsync(demouser, userRole.Name);
    }

    var bookingAdminUser = await _userManager.FindByNameAsync("bookingadmin");
    if (bookingAdminUser == null)
    {
      bookingAdminUser = new AppUser
      {
        UserName = "bookingadmin",
        Email = "bookingadmin@bookingsystem.com",
        EmailConfirmed = true
      };
      var result = await _userManager.CreateAsync(bookingAdminUser, "Pa$$w0rd");
      if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

      result = await _userManager.AddClaimsAsync(bookingAdminUser, new[]
      {
        new Claim(JwtClaimTypes.Name, "Booking Admin"),
        new Claim(JwtClaimTypes.GivenName, "Booking"),
        new Claim(JwtClaimTypes.FamilyName, "Admin"),
        new Claim(JwtClaimTypes.Email, "bookingadmin@bookingsystem.com")
      });
      if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

      if (!await _userManager.IsInRoleAsync(bookingAdminUser, bookingAdminRole.Name))
        await _userManager.AddToRoleAsync(bookingAdminUser, bookingAdminRole.Name);
      if (!await _userManager.IsInRoleAsync(bookingAdminUser, userRole.Name))
        await _userManager.AddToRoleAsync(bookingAdminUser, userRole.Name);
    }

    var configAdminUser = await _userManager.FindByNameAsync("configadmin");
    if (configAdminUser == null)
    {
      configAdminUser = new AppUser
      {
        UserName = "configadmin",
        Email = "configadmin@bookingsystem.com",
        EmailConfirmed = true
      };
      var result = await _userManager.CreateAsync(configAdminUser, "Pa$$w0rd");
      if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

      result = await _userManager.AddClaimsAsync(configAdminUser, new[]
      {
        new Claim(JwtClaimTypes.Name, "Config Admin"),
        new Claim(JwtClaimTypes.GivenName, "Config"),
        new Claim(JwtClaimTypes.FamilyName, "Admin"),
        new Claim(JwtClaimTypes.Email, "configadmin@bookingsystem.com")
      });
      if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

      if (!await _userManager.IsInRoleAsync(configAdminUser, configAdminRole.Name))
        await _userManager.AddToRoleAsync(configAdminUser, configAdminRole.Name);
      if (!await _userManager.IsInRoleAsync(configAdminUser, bookingAdminRole.Name))
        await _userManager.AddToRoleAsync(configAdminUser, bookingAdminRole.Name);
      if (!await _userManager.IsInRoleAsync(configAdminUser, userRole.Name))
        await _userManager.AddToRoleAsync(configAdminUser, userRole.Name);
    }
  }

  protected async Task SeedClients()
  {
    foreach (var client in GetClients())
    {
      var item = await _configurationDbContext.Clients.FirstOrDefaultAsync(c => c.ClientId == client.ClientId);

      if (item == null) await _configurationDbContext.Clients.AddAsync(client.ToEntity());
    }

    await _configurationDbContext.SaveChangesAsync();
  }

  protected async Task SeedApiResources()
  {
    foreach (var apiResource in GetApiResources())
    {
      var item = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(c => c.Name == apiResource.Name);

      if (item == null) await _configurationDbContext.ApiResources.AddAsync(apiResource.ToEntity());
    }

    await _configurationDbContext.SaveChangesAsync();
  }

  protected async Task SeedApiScopes()
  {
    foreach (var apiScope in GetApiScopes())
    {
      var item = await _configurationDbContext.ApiScopes.FirstOrDefaultAsync(c => c.Name == apiScope.Name);

      if (item == null) await _configurationDbContext.ApiScopes.AddAsync(apiScope.ToEntity());
    }

    await _configurationDbContext.SaveChangesAsync();
  }

  protected virtual IEnumerable<ApiResource> GetApiResources()
  {
    return new List<ApiResource>
    {
      new(_audience, "Booking System Api")
      {
        Scopes = new[] { _audience }
      }
    };
  }

  protected virtual IEnumerable<Client> GetClients()
  {
    return new List<Client>
    {
      new()
      {
        ClientId = "booking_system_api_client",
        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
        ClientSecrets = { new Secret("123456".Sha256()) },
        AllowedScopes = { "offline_access", "openid", _audience },
        AccessTokenType = AccessTokenType.Jwt,
        AllowOfflineAccess = true,
        RefreshTokenUsage = TokenUsage.ReUse
      }
    };
  }

  protected virtual IEnumerable<ApiScope> GetApiScopes()
  {
    {
      return new List<ApiScope>
      {
        new("offline_access"),
        new("openid"),
        new(_audience)
      };
    }
  }
}