using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.Persistence.Configurations
{
  public class BookingTeamAreaConfiguration : BaseEntityConfiguration<BookingTeamArea>
  {
    public override void Configure(EntityTypeBuilder<BookingTeamArea> builder)
    {
      base.Configure(builder);

      builder.Property(x => x.Name)
        .IsRequired()
        .HasMaxLength(200);
      builder.Property(x => x.Coords)
        .HasMaxLength(1000);
      builder.HasOne(x => x.BookingLevel)
        .WithMany(x => x.BookingTeamAreas)
        .HasForeignKey(x => x.BookingLevelId);
      builder.HasMany(x => x.BookingObjects)
        .WithOne(x => x.BookingTeamArea);
      builder.Property(x => x.Locked)
        .HasDefaultValue(false);
    }
  }
}