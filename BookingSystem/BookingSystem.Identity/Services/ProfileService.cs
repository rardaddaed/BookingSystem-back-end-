using BookingSystem.Domain.Entities;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace BookingSystem.Identity.Services
{
  public sealed class ProfileService : IProfileService
  {
    private readonly IUserClaimsPrincipalFactory<AppUser> _userClaimsPrincipalFactory;
    private readonly UserManager<AppUser> _userManager;

    public ProfileService(IUserClaimsPrincipalFactory<AppUser> userClaimsPrincipalFactory,
      UserManager<AppUser> userManager)
    {
      _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
      _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
      var sub = context.Subject.GetSubjectId();
      var user = await _userManager.FindByIdAsync(sub);
      var userClaims = await _userClaimsPrincipalFactory.CreateAsync(user);

      var claims = userClaims.Claims.ToList();

      context.IssuedClaims = claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
      var sub = context.Subject.GetSubjectId();
      var user = await _userManager.FindByIdAsync(sub);
      context.IsActive = user != null;
    }
  }
}