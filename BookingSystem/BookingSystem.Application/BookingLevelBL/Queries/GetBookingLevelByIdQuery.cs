using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public Guid BookingLevelId { get; init; }
  }

  public class GetBookingLevelByIdQueryHandler : BaseHandler, IRequestHandler<GetBookingLevelByIdQuery, BookingLevelDto>
  {
    private readonly IMapper _mapper;
    public GetBookingLevelByIdQueryHandler(BSDbContext dbContext, IMapper mapper) : base(dbContext)
    {
      _mapper = mapper;
    }

    public async Task<BookingLevelDto> Handle(GetBookingLevelByIdQuery request, CancellationToken cancellationToken)
    {
      var bookinglevel = await _dbContext.BookingLevels
      .FirstOrDefaultAsync(x => x.BookingLevelId == request.BookingLevelId, cancellationToken);

      if (bookinglevel == null)
      {
        throw new BookingSystemException<Guid>("Booking level not found", request.BookingLevelId);
      }

      var result = _mapper.Map<BookingLevelDto>(bookinglevel);
      return result;
    }
  }
}
