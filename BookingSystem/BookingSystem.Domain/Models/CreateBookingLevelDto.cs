using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Domain.Models
{
  public class CreateBookingLevelDto
  {
    public string Name { get; set; }
    public string Alias { get; set; }
    public string BlueprintUrl { get; set; }
    public int? BlueprintWidth { get; set; }
    public int? BlueprintHeight { get; set; }
    public int MaxBooking { get; set; }
    public bool Locked { get; set; }
  }
}
