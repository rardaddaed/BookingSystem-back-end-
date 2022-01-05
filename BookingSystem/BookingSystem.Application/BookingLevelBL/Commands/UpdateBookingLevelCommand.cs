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
  public class UpdateBookingLevelCommand: IRequest<BookingLevelDto>
  {
    public Guid bookingLevelId { get; init; }
    public string Name { get; init; }
    public string Alias { get; init; }
    public string BlueprintUrl { get; init; }
    public int? BlueprintWidth { get; init; }
    public int? BlueprintHeight { get; init; }
    public int MaxBooking { get; init; }
    public bool Locked { get; init; }

  }

  public class UpdateBookingLevelCommandHandler : BaseHandler, IRequestHandler<UpdateBookingLevelCommand, BookingLevelDto>
  {
    public UpdateBookingLevelCommandHandler(BSDbContext dbContext) : base(dbContext)
    {
    }
    public async Task<BookingLevelDto> Handle(UpdateBookingLevelCommand request, CancellationToken cancellationToken)
    {
      var updatedBookingLevel = await _dbContext.BookingLevels
        .FirstOrDefaultAsync(x => x.BookingLevelId == request.bookingLevelId, cancellationToken);

      if (updatedBookingLevel == null)
      {
        throw new BookingSystemException<Guid>("Booking level not found", request.bookingLevelId);
      }

      updatedBookingLevel.Name = request.Name;
      updatedBookingLevel.Alias = request.Alias;
      updatedBookingLevel.BlueprintUrl = request.BlueprintUrl;
      updatedBookingLevel.BlueprintHeight = request.BlueprintHeight;
      updatedBookingLevel.BlueprintWidth = request.BlueprintWidth;
      updatedBookingLevel.MaxBooking = request.MaxBooking;
      updatedBookingLevel.Locked = request.Locked;

      await _dbContext.SaveChangesAsync(cancellationToken);
      return updatedBookingLevel.AdaptToDto();
    }
    public class UpdateBookingLevelCommandValidator : AbstractValidator<UpdateBookingLevelCommand>
    {
      public UpdateBookingLevelCommandValidator()
      {
        RuleFor(x => x.Name)
          .NotEmpty()
          .MaximumLength(200);
        RuleFor(x => x.Alias)
          .MaximumLength(100);
        RuleFor(x => x.BlueprintUrl)
          .MaximumLength(200);
        RuleFor(x => x.MaxBooking)
          .GreaterThan(0);
      }
    }
  }
}
