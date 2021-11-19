using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace BookingSystem.Application.Infrastructure
{
  public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
  {
    private readonly ILogger<RequestLogger<TRequest>> _logger;

    public RequestLogger(ILogger<RequestLogger<TRequest>> logger)
    {
      _logger = logger;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
      var name = typeof(TRequest).Name;

      _logger.LogInformation($"Request: {name} @{request}", request);

      return Task.CompletedTask;
    }
  }
}