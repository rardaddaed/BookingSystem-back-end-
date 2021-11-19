using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;

namespace BookingSystem.Persistence.Seeders
{
  public class SeederLocal : BaseSeeder
  {
    private readonly ConfigurationDbContext _configurationDbContext;

    public SeederLocal(BSDbContext dbContext, ConfigurationDbContext configurationDbContext) : base(dbContext)
    {
      _configurationDbContext = configurationDbContext;
    }

    public override async Task Seed()
    {
      await base.Seed();

      await SeedIdentity();
    }

    private async Task SeedIdentity()
    {
      #region Initialise

      var clients = new List<Client>
      {
        new Client
        {
          ClientId = "demouser",
          ClientSecrets = new List<Secret> { new Secret("1".ToSha256()) },
          AllowedGrantTypes = new List<string> { GrantType.ClientCredentials },
          AllowedScopes = { "write", "read", "openid", "profile", "email" },
          AccessTokenLifetime = 86400,
          AccessTokenType = AccessTokenType.Jwt
        }
      };

      var apiResources = new List<ApiResource>
      {
        new ApiResource
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
        new ApiScope("openid"),
        new ApiScope("profile"),
        new ApiScope("email"),
        new ApiScope("read"),
        new ApiScope("write")
      };

      #endregion Initialise

      foreach (var client in clients)
      {
        var item = await _configurationDbContext.Clients.FirstOrDefaultAsync(c => c.ClientId == client.ClientId);

        if (item == null)
        {
          await _configurationDbContext.Clients.AddAsync(client.ToEntity());
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
        var item = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(c => c.Name == resource.Name);

        if (item == null)
        {
          await _configurationDbContext.ApiResources.AddAsync(resource.ToEntity());
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
        var item = await _configurationDbContext.ApiScopes.FirstOrDefaultAsync(c => c.Name == scope.Name);

        if (item == null)
        {
          await _configurationDbContext.ApiScopes.AddAsync(scope.ToEntity());
        }
        else
        {
          var scopeEntity = scope.ToEntity();

          item.Name = scopeEntity.Name;
        }
      }

      await _configurationDbContext.SaveChangesAsync();
    }
  }
}