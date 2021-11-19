using Microsoft.AspNetCore.Mvc.Filters;

namespace BookingSystem.WebApi.Filters.Exceptions
{
  public class GlobalExceptionFilter : IExceptionFilter
  {
    private readonly IExceptionResultBuilder _exceptionResultBuilder;

    public GlobalExceptionFilter(IExceptionResultBuilder exceptionResultBuilder)
    {
      _exceptionResultBuilder = exceptionResultBuilder;
    }

    public void OnException(ExceptionContext context)
    {
      var exception = context.Exception;

      var result = _exceptionResultBuilder.Build(exception);

      context.Result = result;
    }
  }
}