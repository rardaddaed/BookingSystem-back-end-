using System;

namespace BookingSystem.Domain.Enums
{
  public enum BookingObjectTypeEnum
  {
    [BookingObjectTypeMetadata(Name = "Desk")]
    Desk,

    [BookingObjectTypeMetadata(Name = "Meeting Room")]
    MeetingRoom
  }

  public class BookingObjectTypeMetadataAttribute : Attribute
  {
    public string Name { get; set; }
  }
}