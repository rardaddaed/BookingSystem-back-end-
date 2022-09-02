using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dapper;
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
using BookingSystem.Repository;

namespace BookingSystem.Application.BookingTeamAreaBL.Queries
{
  public class GetBookingTeamAreaByIdQuery : IRequest<BookingTeamAreaDto>
  {
    public Guid BookingTeamAreaId { get; init; }
  }

  public class GetBookingTeamAreaByIdQueryHandler : IRequestHandler<GetBookingTeamAreaByIdQuery, BookingTeamAreaDto>
  {
    private readonly IBookingTeamAreaRepository _bookingTeamAreaRepository;
    public GetBookingTeamAreaByIdQueryHandler(IBookingTeamAreaRepository bookingTeamAreaRepository)
    {
      _bookingTeamAreaRepository = bookingTeamAreaRepository;
    }

    public async Task<BookingTeamAreaDto> Handle(GetBookingTeamAreaByIdQuery request, CancellationToken cancellationToken)
    {
      return await _bookingTeamAreaRepository.GetByBookingTeamAreaId(request.BookingTeamAreaId);

    }
  }
}
