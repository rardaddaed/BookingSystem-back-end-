using System;

namespace BookingSystem.Domain.Models
{
    public partial record BookingLevelDto
    {
        public Guid BookingLevelId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string BlueprintUrl { get; set; }
        public int? BlueprintWidth { get; set; }
        public int? BlueprintHeight { get; set; }
        public int MaxBooking { get; set; }
        public bool Locked { get; set; }
        public DateTime? CreatedUtc { get; set; }
        public DateTime? ModifiedUtc { get; set; }
    }
}