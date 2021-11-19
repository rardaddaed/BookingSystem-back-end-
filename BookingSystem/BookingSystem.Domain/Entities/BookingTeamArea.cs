using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Domain.Entities
{
  public class BookingTeamArea : BaseEntity
  {
    [Key]
    public Guid BookingTeamAreaId { get; set; }

    public Guid BookingLevelId { get; set; }
    public string Name { get; set; }
    public string Coords { get; set; }
    public bool Locked { get; set; }

    public virtual ICollection<BookingObject> BookingObjects { get; set; }
    public virtual BookingLevel BookingLevel { get; set; }
  }
}