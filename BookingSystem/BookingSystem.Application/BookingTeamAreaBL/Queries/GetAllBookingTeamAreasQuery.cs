using BookingSystem.Application.Infrastructure;
using BookingSystem.Domain.Extensions;
using BookingSystem.Domain.Models;
using BookingSystem.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookingSystem.Application.BookingTeamAreaBL.Queries
{
  public class GetAllBookingTeamAreasQuery : IRequest<IEnumerable<BookingTeamAreaDto>>
  {
  }

  public class GetAllBookingTeamAreasQueryHandler : BaseHandler, IRequestHandler<GetAllBookingTeamAreasQuery, IEnumerable<BookingTeamAreaDto>>
  {
    public GetAllBookingTeamAreasQueryHandler(BSDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<BookingTeamAreaDto>> Handle(GetAllBookingTeamAreasQuery request, CancellationToken cancellationToken)
    {
      var result = await _dbContext.BookingTeamAreas
        .OrderByDescending(x => x.Name)
        .Select(x => new BookingTeamAreaDto {
          BookingTeamAreaId = x.BookingTeamAreaId,
          BookingLevelId = x.BookingLevelId,
          Name = x.Name,
          Coords = x.Coords,
          Locked = x.Locked
        })
        .ToArrayAsync(cancellationToken);
      return result;
    }
  }
}
