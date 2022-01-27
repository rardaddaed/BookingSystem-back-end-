using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Domain.Models
{
  public class UpdateBookingTeamAreaDto
  {
    public Guid BookingTeamAreaId { get; set; }
    public Guid BookingLevelId { get; set; }
    public string Name { get; set; }
    public string Coords { get; set; }
    public bool Locked { get; set; }
  }
}
