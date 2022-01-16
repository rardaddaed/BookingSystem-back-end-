using BookingSystem.Application.Infrastructure;
using BookingSystem.Core.Exceptions;
using BookingSystem.Domain.Extensions;
using BookingSystem.Domain.Models;
using BookingSystem.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookingSystem.Application.BookingTeamAreaBL.Queries
{
  public class GetBookingTeamAreaByIdQuery : IRequest<BookingTeamAreaDto>
  {
    public Guid BookingTeamAreaId { get; init; }
  }

  public class GetBookingTeamAreaByIdQueryHandler : BaseHandler, IRequestHandler<GetBookingTeamAreaByIdQuery, BookingTeamAreaDto>
  {
    public GetBookingTeamAreaByIdQueryHandler(BSDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<BookingTeamAreaDto> Handle(GetBookingTeamAreaByIdQuery request, CancellationToken cancellationToken)
    {
      var result = await _dbContext.BookingTeamAreas
        .Where(x => x.BookingTeamAreaId == request.BookingTeamAreaId)
        .Select(x => new BookingTeamAreaDto
        {
          BookingTeamAreaId = x.BookingTeamAreaId,
          BookingLevelId = x.BookingLevelId,
          Name = x.Name,
          Coords = x.Coords,
          Locked = x.Locked
        })
        .FirstOrDefaultAsync(cancellationToken);

      if (result == null)
      {
        throw new BookingSystemException<Guid>("Booking team area not found", request.BookingTeamAreaId);
      }

      return result;
    }
  }
}
