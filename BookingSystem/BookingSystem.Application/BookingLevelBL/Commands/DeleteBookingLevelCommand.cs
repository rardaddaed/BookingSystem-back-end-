using Dapper;
using BookingSystem.Application.Infrastructure;
using BookingSystem.Core.Exceptions;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Extensions;
using BookingSystem.Domain.Models;
using BookingSystem.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using BookingSystem.Repository;

namespace BookingSystem.Application.BookingLevelBL.Commands
{
  public class DeleteBookingLevelCommand : IRequest
  {
    public Guid BookingLevelId { get; init; }
  }

  public class DeleteBookingLevelCommandHandler : IRequestHandler<DeleteBookingLevelCommand>
  {
    private readonly IBookingLevelRepository _bookingLevelRepository;
    public DeleteBookingLevelCommandHandler(IBookingLevelRepository bookingLevelRepository)
    {
      _bookingLevelRepository = bookingLevelRepository;
    }
    public async Task<Unit> Handle(DeleteBookingLevelCommand request, CancellationToken cancellationToken)
    {
      var bookingLevel = await _bookingLevelRepository.GetByBookingLevelId(request.BookingLevelId);

      if (bookingLevel == null)
      {
        throw new BookingSystemException<Guid>("Booking level not found", request.BookingLevelId);
      }

      await _bookingLevelRepository.DeleteBookingLevel(request.BookingLevelId);

      return Unit.Value;
    }
  }
}
