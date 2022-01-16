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
  public class UpdateBookingTeamAreaCommand : IRequest<BookingTeamAreaDto>
  {
    public Guid BookingTeamAreaId { get; init; }
    public Guid BookingLevelId { get; init; }
    public string Name { get; init; }
    public string Coords { get; init; }
    public bool Locked { get; init; }
  }
  public class UpdateBookingTeamAreaCommandHandler : BaseHandler, IRequestHandler<UpdateBookingTeamAreaCommand, BookingTeamAreaDto>
  {
    public UpdateBookingTeamAreaCommandHandler(BSDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<BookingTeamAreaDto> Handle(UpdateBookingTeamAreaCommand request, CancellationToken cancellationToken)
    {
      var updatedBookingTeamArea = await _dbContext.BookingTeamAreas
        .FirstOrDefaultAsync(x => x.BookingTeamAreaId == request.BookingTeamAreaId, cancellationToken);
      if (updatedBookingTeamArea == null)
      {
        throw new BookingSystemException<Guid>("Booking team area not found", request.BookingTeamAreaId);
      }

      updatedBookingTeamArea.Name = request.Name;
      updatedBookingTeamArea.BookingLevelId = request.BookingLevelId;
      updatedBookingTeamArea.Coords = request.Coords;
      updatedBookingTeamArea.Locked = request.Locked;

      await _dbContext.SaveChangesAsync(cancellationToken);

      var updatedBookingTeamAreaDto = new BookingTeamAreaDto()
      {
        BookingTeamAreaId = updatedBookingTeamArea.BookingTeamAreaId,
        BookingLevelId = updatedBookingTeamArea.BookingLevelId,
        Name = updatedBookingTeamArea.Name,
        Coords = updatedBookingTeamArea.Coords,
        Locked = updatedBookingTeamArea.Locked
      };

      return updatedBookingTeamAreaDto;
    }

    public class UpdateBookingTeamAreaCommandValidator : AbstractValidator<UpdateBookingTeamAreaCommand>
    {
      public UpdateBookingTeamAreaCommandValidator()
      {
        // TODO: check if bookingLevelId exists in database
        RuleFor(x => x.BookingLevelId)
          .NotNull()
          .NotEqual(Guid.Empty);
        RuleFor(x => x.Name)
          .NotEmpty()
          .MaximumLength(200);
        RuleFor(x => x.Coords)
          .MaximumLength(1000);
      }
    }
  }
}
