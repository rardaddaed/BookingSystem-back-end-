using Autofac.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using BookingSystem.Core.Exceptions;

namespace BookingSystem.WebApi.Filters.Exceptions
{
  public class ExceptionResultBuilder : IExceptionResultBuilder
  {
    private readonly ILogger<ExceptionResultBuilder> _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionResultBuilder(ILogger<ExceptionResultBuilder> logger, IWebHostEnvironment env)
    {
      _logger = logger;
      _env = env;
    }

    public IActionResult Build(Exception exception)
    {
      var stackTrace = "No stack trace available";

      // Output StackTrack if it's not production
      if (!_env.IsProduction())
        stackTrace = exception.GetBaseException().StackTrace;

      // Default status code is 500
      var statusCode = 500;
      string content = null;
      var message = exception.GetBaseException().Message;

      // if it is dependency resolution exception
      if (exception is DependencyResolutionException dependencyResolutionException)
        message = $"Dependency Exception: Please ensure that classes implement the interface: {dependencyResolutionException.Message}";

      // all other types of exceptions

      if (exception is BookingSystemException bookingSystemException)
      {
        statusCode = (int)bookingSystemException.StatusCode;
        content = bookingSystemException.GetContent();
      }

      return CreateActionResult(content, message, stackTrace, statusCode, exception);
    }

    protected virtual IActionResult CreateActionResult(string content, string message, string stackTrace,
      int statusCode, Exception exception)
    {
      var apiError = new ApiError
      {
        Message = message,
        Error = content
      };

      if (!string.IsNullOrWhiteSpace(stackTrace))
        apiError.StackTrace = stackTrace;

      var objectResult = new ObjectResult(apiError)
      {
        StatusCode = statusCode
      };
      var eventId = new EventId(statusCode);

      _logger.LogError(exception, $"{eventId.Name}, {message}", objectResult);

      return objectResult;
    }
  }
}