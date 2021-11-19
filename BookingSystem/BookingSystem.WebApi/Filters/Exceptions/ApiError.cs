using Newtonsoft.Json;

namespace BookingSystem.WebApi.Filters.Exceptions
{
  public class ApiError
  {
    public string Message { get; set; }
    public string Error { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string StackTrace { get; set; }
  }
}