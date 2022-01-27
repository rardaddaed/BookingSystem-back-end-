using BookingSystem.Application.Infrastructure;
using BookingSystem.Core.Exceptions;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Extensions;
using BookingSystem.Domain.Models;
using BookingSystem.Persistence;
using Dapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookingSystem.Repository;


namespace BookingSystem.Application.BookingTeamAreaBL.Commands
{
  public class DeleteBookingTeamAreaCommand : IRequest
  {
    public Guid BookingTeamAreaId { get; init; }

  }

  public class DeleteBookingTeamAreaCommandHandler :  IRequestHandler<DeleteBookingTeamAreaCommand>
  {
    private readonly IBookingTeamAreaRepository _bookingTeamAreaRepository;
    public DeleteBookingTeamAreaCommandHandler(IBookingTeamAreaRepository bookingTeamAreaRepository)
    {
      _bookingTeamAreaRepository = bookingTeamAreaRepository;
    }
    public async Task<Unit> Handle(DeleteBookingTeamAreaCommand request, CancellationToken cancellationToken)
    {
      var bookingTeamArea = await _bookingTeamAreaRepository.GetByBookingTeamAreaId(request.BookingTeamAreaId);

      if (bookingTeamArea == null)
      {
        throw new BookingSystemException<Guid>("Booking team area not found", request.BookingTeamAreaId);
      }

      await _bookingTeamAreaRepository.DeleteBookingTeamArea(request.BookingTeamAreaId);

      return Unit.Value;

    }
  }
}
