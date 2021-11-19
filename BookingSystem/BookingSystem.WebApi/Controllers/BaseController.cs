using BookingSystem.Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BookingSystem.WebApi.Controllers
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public abstract class BaseController : Controller
  {
    protected readonly IMediator _mediator;

    protected BaseController(IMediator mediator)
    {
      _mediator = mediator;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
      if (!context.ModelState.IsValid)
      {
        if (!ModelState.IsValid)
        {
          var errorList = ModelState.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Errors.Select(e =>
              string.IsNullOrEmpty(e.ErrorMessage)
                ? e.Exception?.GetBaseException().Message
                : e.ErrorMessage).ToArray()
          );

          throw new BookingSystemException<IDictionary<string, string[]>>(HttpStatusCode.BadRequest,
            "Invalid request - One or more validation failures have occurred.", errorList);
        }
      }
      base.OnActionExecuting(context);
    }
  }
}