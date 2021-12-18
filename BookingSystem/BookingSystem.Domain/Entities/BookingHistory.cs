using System;
using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Domain.Entities
{
  public class BookingHistory: BaseEntity
  {
    [Key]
    public Guid BookingHistoryId { get; set; } 

    public Guid BookingObjectId { get; set; }
    public Guid BookedByAppNetUserId  { get; set; }
    public Guid? CancledByAppNetUserId  { get; set; } 
    public DateTime BookingDateUtc { get; set; }
    public DateTime? CancledDateTimeUtc { get; set; }
    

    public virtual BookingObject BookingObject { get; set; }
  }
}
