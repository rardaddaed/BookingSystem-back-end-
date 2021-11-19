using BookingSystem.Core.Extensions;
using BookingSystem.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using BookingSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Persistence.Seeders
{
  public abstract class BaseSeeder
  {
    protected readonly BSDbContext _dbContext;

    protected BaseSeeder(BSDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public virtual async Task Seed()
    {
      await SeedBookingObjectTypes();
    }

    private async Task SeedBookingObjectTypes()
    {
      foreach (var value in Enum.GetValues(typeof(BookingObjectTypeEnum)).Cast<BookingObjectTypeEnum>())
      {
        var metadata = value.GetAttributeOfType<BookingObjectTypeMetadataAttribute>();
        var bookingObjectType = new BookingObjectType
        {
          BookingObjectTypeId = value,
          Name = metadata.Name
        };

        var dbBookingObjectType = await _dbContext.BookingObjectTypes.FirstOrDefaultAsync(x => x.BookingObjectTypeId == value);
        if (dbBookingObjectType != null)
        {
          dbBookingObjectType.Name = bookingObjectType.Name;
        }
        else
        {
          await _dbContext.BookingObjectTypes.AddAsync(bookingObjectType);
        }

        await _dbContext.SaveChangesAsync();
      }
    }
  }
}