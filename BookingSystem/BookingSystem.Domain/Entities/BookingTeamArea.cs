using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Domain.Entities
{
  public class BookingTeamArea : BaseEntity
  {
    public BookingTeamArea()
    {
      BookingObjects = new List<BookingObject>();
    }

    [Key]
    public Guid BookingTeamAreaId { get; set; }

    public Guid BookingLevelId { get; set; }
    public string Name { get; set; }
    public string Coords { get; set; }
    public bool Locked { get; set; }

    public virtual ICollection<BookingObject> BookingObjects { get; private set; }
    public virtual BookingLevel BookingLevel { get; set; }
  }
}