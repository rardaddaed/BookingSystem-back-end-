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

namespace BookingSystem.Application.BookingLevelBL.Queries
{
  public class GetAllBookingLevelsQuery : IRequest<IEnumerable<BookingLevelDto>>
  {
  }

  public class GetAllBookingLevelsQueryHandler : BaseHandler, IRequestHandler<GetAllBookingLevelsQuery, IEnumerable<BookingLevelDto>>
  {
    public GetAllBookingLevelsQueryHandler(BSDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<BookingLevelDto>> Handle(GetAllBookingLevelsQuery request, CancellationToken cancellationToken)
    {
      return await _dbContext.BookingLevels
        .OrderByDescending(x => x.Alias)
        .Select(BookingLevelMapper.ProjectToDto)
        .ToArrayAsync(cancellationToken);
    }
  }
}