using System.ComponentModel.DataAnnotations;
using BookingSystem.Domain.Enums;

namespace BookingSystem.Domain.Entities
{
  public class BookingObjectType
  {
    [Key]
    public BookingObjectTypeEnum BookingObjectTypeId { get; set; }

    public string Name { get; set; }
  }
}