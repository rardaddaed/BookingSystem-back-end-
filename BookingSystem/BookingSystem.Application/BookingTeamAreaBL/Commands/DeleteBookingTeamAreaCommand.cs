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


namespace BookingSystem.Application.BookingTeamAreaBL.Commands
{
  public class DeleteBookingTeamAreaCommand : IRequest
  {
    public Guid BookingTeamAreaId { get; init; }

  }

  public class DeleteBookingTeamAreaCommandHandler : BaseHandler, IRequestHandler<DeleteBookingTeamAreaCommand>
  {
    public DeleteBookingTeamAreaCommandHandler(BSDbContext dbContext) : base(dbContext)
    {

    }
    public async Task<Unit> Handle(DeleteBookingTeamAreaCommand request, CancellationToken cancellationToken)
    {
      var bookingTeamArea = await _dbContext.BookingTeamAreas
        .FirstOrDefaultAsync(x => x.BookingTeamAreaId == request.BookingTeamAreaId, cancellationToken);

      if (bookingTeamArea == null)
      {
        throw new BookingSystemException<Guid>("Booking team area not found", request.BookingTeamAreaId);
      }
      _dbContext.BookingTeamAreas.Remove(bookingTeamArea);
      await _dbContext.SaveChangesAsync(cancellationToken);

      return Unit.Value;
    }
  }
}
