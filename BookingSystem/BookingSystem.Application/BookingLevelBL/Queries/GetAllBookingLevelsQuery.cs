using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    private readonly IMapper _mapper;

    public GetAllBookingLevelsQueryHandler(BSDbContext dbContext, IMapper mapper) : base(dbContext)
    {
      _mapper = mapper;
    }

    public async Task<IEnumerable<BookingLevelDto>> Handle(GetAllBookingLevelsQuery request, CancellationToken cancellationToken)
    {
      var bookingLevels = await _dbContext.BookingLevels
        .OrderByDescending(x => x.Alias)
        .ToArrayAsync(cancellationToken);

      var result = _mapper.Map<BookingLevelDto[]>(bookingLevels);

      return result;
    }

  }
}