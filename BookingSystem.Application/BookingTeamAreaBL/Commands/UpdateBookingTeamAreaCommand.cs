using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dapper;
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
using System.Linq;
using BookingSystem.Repository;

namespace BookingSystem.Application.BookingTeamAreaBL.Commands
{
  public class UpdateBookingTeamAreaCommand : IRequest<BookingTeamAreaDto>
  {
    public UpdateBookingTeamAreaDto UpdateBookingTeamAreaDto { get; set; }
    public UpdateBookingTeamAreaCommand(UpdateBookingTeamAreaDto updateBookingTeamAreaDto)
    {
      UpdateBookingTeamAreaDto = updateBookingTeamAreaDto;  
    }
  }
  public class UpdateBookingTeamAreaCommandHandler : IRequestHandler<UpdateBookingTeamAreaCommand, BookingTeamAreaDto>
  {
    private readonly IBookingTeamAreaRepository _bookingTeamAreaRepository;
    public UpdateBookingTeamAreaCommandHandler(IBookingTeamAreaRepository bookingTeamAreaRepository)
    {
      _bookingTeamAreaRepository = bookingTeamAreaRepository;
    }

    public async Task<BookingTeamAreaDto> Handle(UpdateBookingTeamAreaCommand request, CancellationToken cancellationToken)
    {
      var bookingTeamArea = await _bookingTeamAreaRepository.GetByBookingTeamAreaId(request.UpdateBookingTeamAreaDto.BookingTeamAreaId);

      if (bookingTeamArea == null)
      {
        throw new BookingSystemException<Guid>("Booking team area not found", request.UpdateBookingTeamAreaDto.BookingTeamAreaId);
      }

      await _bookingTeamAreaRepository.UpdateBookingTeamArea(request.UpdateBookingTeamAreaDto);

      return await _bookingTeamAreaRepository.GetByBookingTeamAreaId(request.UpdateBookingTeamAreaDto.BookingTeamAreaId);
    }
    }
  public class UpdateBookingTeamAreaCommandValidator : AbstractValidator<UpdateBookingTeamAreaCommand>
  {
    public UpdateBookingTeamAreaCommandValidator()
    {
      // TODO: check if bookingLevelId exists in database
      RuleFor(x => x.UpdateBookingTeamAreaDto.BookingLevelId)
        .NotNull()
        .NotEqual(Guid.Empty);
      RuleFor(x => x.UpdateBookingTeamAreaDto.Name)
        .NotEmpty()
        .MaximumLength(200);
      RuleFor(x => x.UpdateBookingTeamAreaDto.Coords)
        .MaximumLength(1000);
    }
  }
}
