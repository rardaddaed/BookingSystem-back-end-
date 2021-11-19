using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.Persistence.Configurations
{
  public class BookingLevelConfiguration : BaseEntityConfiguration<BookingLevel>
  {
    public override void Configure(EntityTypeBuilder<BookingLevel> builder)
    {
      base.Configure(builder);

      builder.Property(x => x.Name)
        .IsRequired()
        .HasMaxLength(200);
      builder.Property(x => x.Alias)
        .HasMaxLength(100);
      builder.Property(x => x.BlueprintUrl)
        .HasMaxLength(200);
      builder.Property(x => x.MaxBooking)
        .HasDefaultValue(8);
      builder.HasMany(x => x.BookingTeamAreas)
        .WithOne(x => x.BookingLevel);
      builder.HasMany(x => x.BookingObjects)
        .WithOne(x => x.BookingLevel);
      builder.Property(x => x.Locked)
        .HasDefaultValue(false);
    }
  }
}