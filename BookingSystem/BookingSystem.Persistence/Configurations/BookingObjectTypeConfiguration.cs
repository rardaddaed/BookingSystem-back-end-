using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.Persistence.Configurations
{
  public class BookingObjectTypeConfiguration : IEntityTypeConfiguration<BookingObjectType>
  {
    public void Configure(EntityTypeBuilder<BookingObjectType> builder)
    {
      builder.Property(x => x.Name)
        .HasMaxLength(50);
    }
  }
}