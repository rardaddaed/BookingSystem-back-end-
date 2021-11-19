using Microsoft.AspNetCore.Mvc;
using System;

namespace BookingSystem.WebApi.Filters.Exceptions
{
  public interface IExceptionResultBuilder
  {
    IActionResult Build(Exception exception);
  }
}