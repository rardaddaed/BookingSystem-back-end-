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
using Dapper;
using BookingSystem.Repository;

namespace BookingSystem.Application.BookingLevelBL.Queries
{
  public class GetBookingLevelByIdQuery : IRequest<BookingLevelDto>
  {
    public Guid BookingLevelId { get; init; }
  }

  public class GetBookingLevelByIdQueryHandler : IRequestHandler<GetBookingLevelByIdQuery, BookingLevelDto>
  {
    private readonly IBookingLevelRepository _bookingLevelRepository;
    public GetBookingLevelByIdQueryHandler(IBookingLevelRepository bookingLevelRepository)
    {
      _bookingLevelRepository = bookingLevelRepository;
    }

    public async Task<BookingLevelDto> Handle(GetBookingLevelByIdQuery request, CancellationToken cancellationToken)
    {
      return await _bookingLevelRepository.GetByBookingLevelId(request.BookingLevelId);

    }
  }
}
