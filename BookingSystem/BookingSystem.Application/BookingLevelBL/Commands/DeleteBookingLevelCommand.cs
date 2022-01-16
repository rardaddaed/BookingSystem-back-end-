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


namespace BookingSystem.Application.BookingLevelBL.Commands
{
  public class DeleteBookingLevelCommand : IRequest
  {
    public Guid BookingLevelId { get; init; }
  }

  public class DeleteBookingLevelCommandHandler : BaseHandler, IRequestHandler<DeleteBookingLevelCommand>
  {
    public DeleteBookingLevelCommandHandler(BSDbContext dbContext) : base(dbContext)
    {

    }
    public async Task<Unit> Handle(DeleteBookingLevelCommand request, CancellationToken cancellationToken)
    {
      var bookingLevel = await _dbContext.BookingLevels
        .FirstOrDefaultAsync(x => x.BookingLevelId == request.BookingLevelId, cancellationToken);

      if (bookingLevel == null)
      {
        throw new BookingSystemException<Guid>("Booking level not found", request.BookingLevelId);
      }
      _dbContext.BookingLevels.Remove(bookingLevel);
      await _dbContext.SaveChangesAsync(cancellationToken);

      return Unit.Value;
    }
  }
}
