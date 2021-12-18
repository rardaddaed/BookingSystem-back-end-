using BookingSystem.Core.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using BookingSystem.Domain.Enums;
using System.Collections.Generic;

namespace BookingSystem.Domain.Entities
{
  public class BookingObject : BaseEntity
  {
    public BookingObject()
    {
      BookingHistories = new List<BookingHistory>();
    }

    [Key]
    public Guid BookingObjectId { get; set; }

    public BookingObjectTypeEnum BookingObjectTypeId { get; set; }

    public string BookingObjectType =>
      BookingObjectTypeId.GetAttributeOfType<BookingObjectTypeMetadataAttribute>().Name;

    public Guid BookingLevelId { get; set; }
    public Guid? BookingTeamAreaId { get; set; }
    public string Name { get; set; }
    public string Coords { get; set; }
    public bool Locked { get; set; } // removed?                                                                                                                                                                                                                              

    public virtual BookingLevel BookingLevel { get; set; }
    public virtual BookingTeamArea BookingTeamArea { get; set; }
    public virtual ICollection<BookingHistory> BookingHistories { get; private set; }
  }
}