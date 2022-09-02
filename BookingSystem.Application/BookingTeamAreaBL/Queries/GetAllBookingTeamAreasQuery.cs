using AutoMapper;
using Dapper;
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
using BookingSystem.Repository;

namespace BookingSystem.Application.BookingTeamAreaBL.Queries
{
  public class GetAllBookingTeamAreasQuery : IRequest<IEnumerable<BookingTeamAreaDto>>
  {
  }

  public class GetAllBookingTeamAreasQueryHandler : IRequestHandler<GetAllBookingTeamAreasQuery, IEnumerable<BookingTeamAreaDto>>
  {
    private readonly IBookingTeamAreaRepository _bookingTeamAreaRepository;
    public GetAllBookingTeamAreasQueryHandler(IBookingTeamAreaRepository bookingTeamAreaRepository)
    {
      _bookingTeamAreaRepository = bookingTeamAreaRepository;
    }

    public async Task<IEnumerable<BookingTeamAreaDto>> Handle(GetAllBookingTeamAreasQuery request, CancellationToken cancellationToken)
    {
      return await _bookingTeamAreaRepository.GetAll();
    }
  }
}
