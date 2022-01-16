using System;

namespace BookingSystem.Domain.Models
{
  public partial record BookingTeamAreaDto
  {
    public Guid BookingTeamAreaId { get; set; }
    public Guid BookingLevelId { get; set; }
    public string Name { get; set; }
    public string Coords { get; set; }
    public bool Locked { get; set; }
  }
}