using BookingSystem.Application.BookingLevelBL.Commands;
using BookingSystem.Application.BookingLevelBL.Queries;
using BookingSystem.Application.BookingTeamAreaBL.Commands;
using BookingSystem.Application.BookingTeamAreaBL.Queries;
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
  public class BookingTeamAreaController : BaseController
  {
    public BookingTeamAreaController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [Authorize(Policy = "ConfigAdmin")]
    [ProducesResponseType(typeof(IEnumerable<BookingTeamAreaDto>), 200)]
    public async Task<ActionResult<IEnumerable<BookingTeamAreaDto>>> GetAll()
    {
      return Ok(await _mediator.Send(new GetAllBookingTeamAreasQuery()));
    }

    [Route("{bookingTeamAreaId}")]
    [HttpGet]
    [Authorize(Policy = "ConfigAdmin")]
    [ProducesResponseType(typeof(BookingTeamAreaDto), 200)]
    public async Task<ActionResult<BookingTeamAreaDto>> GetById(Guid bookingTeamAreaId)
    {
      return Ok(await _mediator.Send(new GetBookingTeamAreaByIdQuery { BookingTeamAreaId = bookingTeamAreaId }));
    }

    [Route("create")]
    [HttpPost]
    [Authorize(Policy = "ConfigAdmin")]
    [ProducesResponseType(typeof(BookingTeamAreaDto), 200)]
    public async Task<ActionResult<BookingTeamAreaDto>> Create([FromBody] CreateBookingTeamAreaDto commandDto)
    {
      return Ok(await _mediator.Send(new CreateBookingTeamAreaCommand(commandDto)));
    }

    [Route("update")]
    [HttpPut]
    [Authorize(Policy = "ConfigAdmin")]
    [ProducesResponseType(typeof(BookingTeamAreaDto), 200)]
    public async Task<ActionResult<BookingTeamAreaDto>> Update([FromBody] UpdateBookingTeamAreaDto commandDto)
    {
      return Ok(await _mediator.Send(new UpdateBookingTeamAreaCommand(commandDto)));
    }

    [Route("delete/{bookingTeamAreaId}")]
    [HttpDelete]
    [Authorize(Policy = "ConfigAdmin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(Guid bookingTeamAreaId)
    {
      await _mediator.Send(new DeleteBookingTeamAreaCommand { BookingTeamAreaId = bookingTeamAreaId });
      return NoContent();
    }
  }
}
