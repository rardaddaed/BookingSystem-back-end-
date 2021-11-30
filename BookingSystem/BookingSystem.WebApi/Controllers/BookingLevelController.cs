using BookingSystem.Application.BookingLevelBL.Queries;
using BookingSystem.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize(Policy = "User")]
    [ProducesResponseType(typeof(IEnumerable<BookingLevelDto>), 200)]
    public async Task<ActionResult<IEnumerable<BookingLevelDto>>> GetAll()
    {
      return Ok(await _mediator.Send(new GetAllBookingLevelsQuery()));
    }
  }
}