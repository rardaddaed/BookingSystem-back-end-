using System;
using System.Collections.Generic;
using System.Text;
using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.Persistence.Configurations
{
  public class BookingObjectConfiguration : BaseEntityConfiguration<BookingObject>
  {
    public override void Configure(EntityTypeBuilder<BookingObject> builder)
    {
      base.Configure(builder);

      builder.Property(x => x.Name)
        .IsRequired()
        .HasMaxLength(100);
      builder.Property(x => x.Coords)
        .HasMaxLength(1000);
      builder.HasOne(x => x.BookingLevel)
        .WithMany(x => x.BookingObjects)
        .HasForeignKey(x => x.BookingLevelId);
      builder.HasOne(x => x.BookingTeamArea)
        .WithMany(x => x.BookingObjects)
        .HasForeignKey(x => x.BookingTeamAreaId);
      builder.Property(x => x.Locked)
        .HasDefaultValue(false);
    }
  }
}