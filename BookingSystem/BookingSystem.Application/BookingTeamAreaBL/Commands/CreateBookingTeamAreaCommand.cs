using BookingSystem.Application.Infrastructure;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Extensions;
using BookingSystem.Domain.Models;
using BookingSystem.Persistence;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace BookingSystem.Application.BookingTeamAreaBL.Commands
{
  public class CreateBookingTeamAreaCommand : IRequest<BookingTeamAreaDto>
  {
    public Guid BookingLevelId { get; init; }
    public string Name { get; init; }
    public string Coords { get; init; }
    public bool Locked { get; init; }
  }

  public class CreateBookingTeamAreaCommandHandler : BaseHandler, IRequestHandler<CreateBookingTeamAreaCommand, BookingTeamAreaDto>
  {
    public CreateBookingTeamAreaCommandHandler(BSDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<BookingTeamAreaDto> Handle(CreateBookingTeamAreaCommand request, CancellationToken cancellationToken)
    {
      var newBookingTeamArea = new BookingTeamArea()
      {
        BookingTeamAreaId = Guid.NewGuid(),
        BookingLevelId = request.BookingLevelId,
        Name = request.Name,
        Coords = request.Coords,
        Locked = request.Locked
      };

      _dbContext.BookingTeamAreas.Add(newBookingTeamArea);
      await _dbContext.SaveChangesAsync(cancellationToken);

      var newBookingTeamAreaDto = new BookingTeamAreaDto()
      {
        BookingTeamAreaId = newBookingTeamArea.BookingTeamAreaId,
        BookingLevelId = newBookingTeamArea.BookingLevelId,
        Name = newBookingTeamArea.Name,
        Coords = newBookingTeamArea.Coords,
        Locked = newBookingTeamArea.Locked
      };

      return newBookingTeamAreaDto;
    }

    public class CreateBookingTeamAreaCommandValidator : AbstractValidator<CreateBookingTeamAreaCommand>
    {
      public CreateBookingTeamAreaCommandValidator()
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
