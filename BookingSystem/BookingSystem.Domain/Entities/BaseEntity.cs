using System;

namespace BookingSystem.Domain.Entities
{
  public abstract class BaseEntity
  {
    public DateTime? CreatedUtc { get; set; }
    public DateTime? ModifiedUtc { get; set; }
  }
}