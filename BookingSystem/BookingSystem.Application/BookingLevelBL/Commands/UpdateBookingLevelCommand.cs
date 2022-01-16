using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public UpdateBookingLevelDto UpdateBookingLevelDto { get; set; }
    public UpdateBookingLevelCommand(UpdateBookingLevelDto updateBookingLevelDto)
    {
      UpdateBookingLevelDto = updateBookingLevelDto;  
    }
  }

  public class UpdateBookingLevelCommandHandler : BaseHandler, IRequestHandler<UpdateBookingLevelCommand, BookingLevelDto>
  {
    private readonly IMapper _mapper;
    public UpdateBookingLevelCommandHandler(BSDbContext dbContext, IMapper mapper) : base(dbContext)
    {
      _mapper = mapper;
    }
    public async Task<BookingLevelDto> Handle(UpdateBookingLevelCommand request, CancellationToken cancellationToken)
    {
      var bookingLevel = await _dbContext.BookingLevels
        .FirstOrDefaultAsync(x => x.BookingLevelId == request.UpdateBookingLevelDto.bookingLevelId, cancellationToken);

      if (bookingLevel == null)
      {
        throw new BookingSystemException<Guid>("Booking level not found", request.UpdateBookingLevelDto.bookingLevelId);
      }

      var newBookingLevel = _mapper.Map<BookingLevel>(request.UpdateBookingLevelDto);

      var result = _mapper.Map<BookingLevelDto>(newBookingLevel);

      await _dbContext.SaveChangesAsync(cancellationToken);
      return result;
    }
    public class UpdateBookingLevelCommandValidator : AbstractValidator<UpdateBookingLevelCommand>
    {
      public UpdateBookingLevelCommandValidator()
      {
        RuleFor(x => x.UpdateBookingLevelDto.Name)
          .NotEmpty()
          .MaximumLength(200);
        RuleFor(x => x.UpdateBookingLevelDto.Alias)
          .MaximumLength(100);
        RuleFor(x => x.UpdateBookingLevelDto.BlueprintUrl)
          .MaximumLength(200);
        RuleFor(x => x.UpdateBookingLevelDto.MaxBooking)
          .GreaterThan(0);
      }
    }
  }
}
