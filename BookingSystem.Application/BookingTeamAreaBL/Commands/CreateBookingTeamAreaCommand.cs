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

namespace BookingSystem.Application.BookingTeamAreaBL.Commands
{
  public class CreateBookingTeamAreaCommand : IRequest<BookingTeamAreaDto>
  {
    public CreateBookingTeamAreaDto CreateBookingTeamAreaDto { get; set; }
    public CreateBookingTeamAreaCommand(CreateBookingTeamAreaDto createBookingTeamAreaDto)
    {
      CreateBookingTeamAreaDto = createBookingTeamAreaDto;
    }
  }

  public class CreateBookingTeamAreaCommandHandler : IRequestHandler<CreateBookingTeamAreaCommand, BookingTeamAreaDto>
  {
    private readonly IBookingTeamAreaRepository _bookingTeamAreaRepository;
    public CreateBookingTeamAreaCommandHandler(IBookingTeamAreaRepository bookingTeamAreaRepository)
    {
      _bookingTeamAreaRepository = bookingTeamAreaRepository;
    }

    public async Task<BookingTeamAreaDto> Handle(CreateBookingTeamAreaCommand request, CancellationToken cancellationToken)
    {
      var bookingTeamAreaId = Guid.NewGuid();
      
      await _bookingTeamAreaRepository.CreateBookingTeamArea(request.CreateBookingTeamAreaDto, bookingTeamAreaId);

      return await _bookingTeamAreaRepository.GetByBookingTeamAreaId(bookingTeamAreaId);
    }
    }
  public class CreateBookingTeamAreaCommandValidator : AbstractValidator<CreateBookingTeamAreaCommand>
  {
    public CreateBookingTeamAreaCommandValidator()
    {
      // TODO: check if bookingLevelId exists in database
      RuleFor(x => x.CreateBookingTeamAreaDto.BookingLevelId)
        .NotNull()
        .NotEqual(Guid.Empty);
      RuleFor(x => x.CreateBookingTeamAreaDto.Name)
        .NotEmpty()
        .MaximumLength(200);
      RuleFor(x => x.CreateBookingTeamAreaDto.Coords)
        .MaximumLength(1000);
    }
  }
}
