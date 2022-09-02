using AutoMapper;
using Dapper;
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
using BookingSystem.Repository;

namespace BookingSystem.Application.BookingLevelBL.Queries
{
  public class GetAllBookingLevelsQuery : IRequest<IEnumerable<BookingLevelDto>>
  {
  }

  public class GetAllBookingLevelsQueryHandler : IRequestHandler<GetAllBookingLevelsQuery, IEnumerable<BookingLevelDto>>
  {
    private readonly IBookingLevelRepository _bookingLevelRepository;
    public GetAllBookingLevelsQueryHandler(IBookingLevelRepository bookingLevelRepository)
    {
      _bookingLevelRepository = bookingLevelRepository;
    }

    public async Task<IEnumerable<BookingLevelDto>> Handle(GetAllBookingLevelsQuery request, CancellationToken cancellationToken)
    {

      return await _bookingLevelRepository.GetAll();

    }

  }
}