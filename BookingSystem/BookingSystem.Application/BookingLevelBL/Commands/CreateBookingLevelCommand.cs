using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public CreateBookingLevelDto CreateBookingLevelDto { get; set; }
    public CreateBookingLevelCommand(CreateBookingLevelDto createBookingLevelDto)
    {
      CreateBookingLevelDto = createBookingLevelDto;
    }
  }

  public class CreateBookingLevelCommandHandler : BaseHandler, IRequestHandler<CreateBookingLevelCommand, BookingLevelDto>
  {
    private readonly IMapper _mapper;
    public CreateBookingLevelCommandHandler(BSDbContext dbContext, IMapper mapper) : base(dbContext)
    {
      _mapper = mapper;
    }

    public async Task<BookingLevelDto> Handle(CreateBookingLevelCommand request, CancellationToken cancellationToken)
    {
      var newBookingLevel = _mapper.Map<BookingLevel>(request.CreateBookingLevelDto);

      _dbContext.BookingLevels.Add(newBookingLevel);
      await _dbContext.SaveChangesAsync(cancellationToken);

      var result = _mapper.Map<BookingLevelDto>(newBookingLevel);

      return result;
    }
  }

  public class CreateBookingLevelCommandValidator : AbstractValidator<CreateBookingLevelCommand>
  {
    public CreateBookingLevelCommandValidator()
    {
      RuleFor(x => x.CreateBookingLevelDto.Name)
        .NotEmpty()
        .MaximumLength(200);
      RuleFor(x => x.CreateBookingLevelDto.Alias)
        .MaximumLength(100);
      RuleFor(x => x.CreateBookingLevelDto.BlueprintUrl)
        .MaximumLength(200);
      RuleFor(x => x.CreateBookingLevelDto.MaxBooking)
        .GreaterThan(0);
    }
  }
}
