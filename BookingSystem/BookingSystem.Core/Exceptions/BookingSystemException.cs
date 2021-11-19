using Newtonsoft.Json;
using System;
using System.Net;

namespace BookingSystem.Core.Exceptions
{
  public abstract class BookingSystemException : Exception
  {
    public HttpStatusCode StatusCode { get; set; }
    protected object InternalContent { get; set; }

    protected BookingSystemException(string message, object context = null) : this(HttpStatusCode.BadRequest, message, null, context)
    {
    }

    protected BookingSystemException(HttpStatusCode statusCode, string message, object context = null) : this(statusCode, message, null, context)
    {
    }

    protected BookingSystemException(HttpStatusCode statusCode, string message, Exception innerException,
      object content = null) : base(message, innerException)
    {
      StatusCode = statusCode;
      InternalContent = content;
    }

    public abstract string GetContent();

    public virtual object GetRawContent()
    {
      return InternalContent;
    }
  }

  public class BookingSystemException<TContent> : BookingSystemException, IBookingSystemException<TContent>
  {
    public BookingSystemException(string message, TContent context = default) : this(HttpStatusCode.BadRequest, message, null, context)
    {
    }

    public BookingSystemException(HttpStatusCode statusCode, string message, TContent context = default) : this(statusCode, message, null, context)
    {
    }

    public BookingSystemException(HttpStatusCode statusCode, string message, Exception innerException, TContent content = default)
      : base(statusCode, message, innerException, content)
    {
    }

    public override string GetContent()
    {
      if (Content != null)
      {
        var body = JsonConvert.SerializeObject(Content);
        return body;
      }

      return null;
    }

    public TContent Content
    {
      get => (TContent)InternalContent;
      set => InternalContent = value;
    }
  }
}