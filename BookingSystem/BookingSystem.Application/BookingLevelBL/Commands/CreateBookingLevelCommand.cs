using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dapper;
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
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BookingSystem.Repository;

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

  public class CreateBookingLevelCommandHandler : IRequestHandler<CreateBookingLevelCommand, BookingLevelDto>
  {
    private readonly IBookingLevelRepository _bookingLevelRepository;
    public CreateBookingLevelCommandHandler(IBookingLevelRepository bookingLevelRepository)
    {
      _bookingLevelRepository = bookingLevelRepository;
    }

    public async Task<BookingLevelDto> Handle(CreateBookingLevelCommand request, CancellationToken cancellationToken)
    {
      var bookingLevelId = Guid.NewGuid();

      await _bookingLevelRepository.CreateBookingLevel(request.CreateBookingLevelDto, bookingLevelId);

      return await _bookingLevelRepository.GetByBookingLevelId(bookingLevelId);
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
