using System.Net;

namespace BookingSystem.Core.Exceptions
{
  public interface IBookingSystemException<TContent>
  {
    HttpStatusCode StatusCode { get; set; }
    TContent Content { get; set; }
  }
}