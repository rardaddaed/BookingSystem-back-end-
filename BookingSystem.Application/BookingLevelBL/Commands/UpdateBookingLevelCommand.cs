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

  public class UpdateBookingLevelCommandHandler : IRequestHandler<UpdateBookingLevelCommand, BookingLevelDto>
  {
    private readonly IBookingLevelRepository _bookingLevelRepository;
    public UpdateBookingLevelCommandHandler(IBookingLevelRepository bookingLevelRepository)
    {
      _bookingLevelRepository = bookingLevelRepository; 
    }
    public async Task<BookingLevelDto> Handle(UpdateBookingLevelCommand request, CancellationToken cancellationToken)
    {
      var bookingLevel = await _bookingLevelRepository.GetByBookingLevelId(request.UpdateBookingLevelDto.BookingLevelId);

      if (bookingLevel == null)
      {
        throw new BookingSystemException<Guid>("Booking level not found", request.UpdateBookingLevelDto.BookingLevelId);
      }

      await _bookingLevelRepository.UpdateBookingLevel(request.UpdateBookingLevelDto);

      return await _bookingLevelRepository.GetByBookingLevelId(request.UpdateBookingLevelDto.BookingLevelId);
    }
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
