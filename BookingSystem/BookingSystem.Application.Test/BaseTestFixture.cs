using AutoFixture;
using BookingSystem.Domain.Entities;
using BookingSystem.Persistence;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace BookingSystem.Application.Test
{
  public class BaseTestFixture : IAsyncDisposable
  {
    protected BSDbContext _dbContext;
    protected BookingLevel _level1;
    protected BookingLevel _level2;
    protected BookingLevel _level3;

    [SetUp]
    public async Task SetUpDbContext()
    {
      var fixture = new Fixture();
      fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
      fixture.Behaviors.Add(new OmitOnRecursionBehavior());

      _level1 = fixture.Create<BookingLevel>();
      _level2 = fixture.Create<BookingLevel>();
      _level3 = fixture.Create<BookingLevel>();

      var options = new DbContextOptionsBuilder<BSDbContext>()
        .UseInMemoryDatabase("Test")
        .Options;
      _dbContext = new BSDbContext(options);

      await _dbContext.BookingLevels.AddRangeAsync(getBookingLevels());

      await _dbContext.SaveChangesAsync();
    }

    protected BookingLevel[] getBookingLevels()
    {
      return new[] { _level1, _level2, _level3 };
    }

    public ValueTask DisposeAsync()
    {
      return _dbContext.DisposeAsync();
    }
  }
}