using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Domain.Entities
{
  public class BookingLevel : BaseEntity
  {
    [Key]
    public Guid BookingLevelId { get; set; }

    public string Name { get; set; }
    public string Alias { get; set; }
    public string BlueprintUrl { get; set; }
    public int? BlueprintWidth { get; set; }
    public int? BlueprintHeight { get; set; }
    public int MaxBooking { get; set; }
    public bool Locked { get; set; }

    public virtual ICollection<BookingTeamArea> BookingTeamAreas { get; set; }
    public virtual ICollection<BookingObject> BookingObjects { get; set; }
  }
}