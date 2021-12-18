using System;
using System.Collections.Generic;
using System.Text;
using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.Persistence.Configurations
{
  public class BookingHistoryCOnfiguration : BaseEntityConfiguration<BookingHistory>
  {
    public override void Configure(EntityTypeBuilder<BookingHistory> builder)
    {
      base.Configure(builder);

      builder.HasOne(x => x.BookingObject)
        .WithMany(x => x.BookingHistories)
        .HasForeignKey(x => x.BookingObjectId);

    }
  }
}