using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.Persistence.Configurations
{
  public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
  {
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
      //builder.Property(x => x.Deleted)
      //  .IsRequired()
      //  .HasDefaultValue(false);
    }
  }
}