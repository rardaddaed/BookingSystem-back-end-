using BookingSystem.Application.Infrastructure;
using BookingSystem.Domain.Extensions;
using BookingSystem.Domain.Models;
using BookingSystem.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using BookingSystem.Core.Exceptions;

namespace BookingSystem.Application.BookingLevelBL.Queries
{
  public class GetBookingLevelByIdQuery : IRequest<BookingLevelDto>
  {

    public Guid bookingLevelId;
    public GetBookingLevelByIdQuery(Guid bookingLevelId)
    {
      this.bookingLevelId = bookingLevelId;
    }
  }

  public class GetBookingLevelByIdQueryHandler : BaseHandler, IRequestHandler<GetBookingLevelByIdQuery, BookingLevelDto>
  {
    public GetBookingLevelByIdQueryHandler(BSDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<BookingLevelDto> Handle(GetBookingLevelByIdQuery request, CancellationToken cancellationToken)
    { 
        var result = (await _dbContext.BookingLevels
        .FirstOrDefaultAsync(x => x.BookingLevelId == request.bookingLevelId, cancellationToken))
        .AdaptToDto();

      if (result == null)
      {
        throw new BookingSystemException<Guid>("Booking level not found", request.bookingLevelId);
      }

      return result;
    }
  }
}
