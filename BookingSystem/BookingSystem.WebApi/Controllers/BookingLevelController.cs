﻿using BookingSystem.Application.BookingLevelBL.Commands;
using BookingSystem.Application.BookingLevelBL.Queries;
using BookingSystem.Core.Exceptions;
using BookingSystem.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingSystem.WebApi.Controllers
{
  public class BookingLevelController : BaseController
  {
    public BookingLevelController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [Authorize(Policy = "ConfigAdmin")]
    [ProducesResponseType(typeof(IEnumerable<BookingLevelDto>), 200)]
    public async Task<ActionResult<IEnumerable<BookingLevelDto>>> GetAll()
    {
      return Ok(await _mediator.Send(new GetAllBookingLevelsQuery()));
    }

    [Route("{bookingLevelId}")]
    [HttpGet]
    [Authorize(Policy = "ConfigAdmin")]
    [ProducesResponseType(typeof(BookingLevelDto), 200)]
    public async Task<ActionResult<BookingLevelDto>> GetById(Guid bookingLevelId)
    {
      return Ok(await _mediator.Send(new GetBookingLevelByIdQuery(bookingLevelId)));
    }

    [HttpPost]
    [Authorize(Policy = "ConfigAdmin")]
    [ProducesResponseType(typeof(BookingLevelDto), 200)]
    public async Task<ActionResult<BookingLevelDto>> Create([FromBody] CreateBookingLevelCommand command)
    {
      return Ok(await _mediator.Send(command));
    }

    [Route("{bookingLevelId}")]
    [HttpPut]
    [Authorize(Policy = "ConfigAdmin")]
    [ProducesResponseType(typeof(BookingLevelDto), 200)]
    public async Task<ActionResult<BookingLevelDto>> Update([FromBody] UpdateBookingLevelCommand command, Guid bookingLevelId)
    {
      if(command.bookingLevelId != bookingLevelId)
      {
        return BadRequest();
      }
      return Ok(await _mediator.Send(command));
    }

    [Route("{bookingLevelId}")]
    [HttpDelete]
    [Authorize(Policy = "ConfigAdmin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(Guid bookingLevelId)
    {
      await _mediator.Send(new DeleteBookingLevelCommand(bookingLevelId));
      return NoContent();
    }
  }
}