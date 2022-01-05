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

namespace BookingSystem.Application.BookingLevelBL.Commands
{
  public class CreateBookingLevelCommand : IRequest<BookingLevelDto>
  {
    public string Name { get; set; }
    public string Alias { get; set; }
    public string BlueprintUrl { get; set; }
    public int? BlueprintWidth { get; set; }
    public int? BlueprintHeight { get; set; }
    public int MaxBooking { get; set; }
    public bool Locked { get; set; }

  }

  public class CreateBookingLevelCommandHandler : BaseHandler, IRequestHandler<CreateBookingLevelCommand, BookingLevelDto>
  {
    public CreateBookingLevelCommandHandler(BSDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<BookingLevelDto> Handle(CreateBookingLevelCommand request, CancellationToken cancellationToken)
    {
      var newBookingLevel = new BookingLevel()
      {
        BookingLevelId = Guid.NewGuid(),
        Name = request.Name,
        Alias = request.Alias,
        BlueprintUrl = request.BlueprintUrl,
        BlueprintWidth = request.BlueprintWidth,
        BlueprintHeight = request.BlueprintHeight,
        MaxBooking = request.MaxBooking,
        Locked = request.Locked
      };

      _dbContext.BookingLevels.Add(newBookingLevel);
      await _dbContext.SaveChangesAsync(cancellationToken);

      return newBookingLevel.AdaptToDto();
    }
  }

  public class CreateBookingLevelCommandValidator : AbstractValidator<CreateBookingLevelCommand>
  {
    public CreateBookingLevelCommandValidator()
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
      RuleFor(x => x.Locked)
          .Equal(false);
    }
  }
}
