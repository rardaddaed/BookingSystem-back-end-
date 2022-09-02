using Microsoft.AspNetCore.SignalR;

namespace BookingSystem.Application.Infrastructure.SignalR
{
  public class BookingHub : Hub<IBookingHubClient>
  {
  }
}