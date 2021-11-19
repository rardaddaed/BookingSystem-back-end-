using AutoFixture;
using BookingSystem.Domain.Models;
using NUnit.Framework;

namespace BookingSystem.WebApi.Test
{
  public class BaseTestFixture
  {
    protected BookingLevelDto _levelDto1;
    protected BookingLevelDto _levelDto2;
    protected BookingLevelDto _levelDto3;

    [SetUp]
    public void SetUpDbContext()
    {
      var fixture = new Fixture();
      fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
      fixture.Behaviors.Add(new OmitOnRecursionBehavior());

      _levelDto1 = fixture.Create<BookingLevelDto>();
      _levelDto2 = fixture.Create<BookingLevelDto>();
      _levelDto3 = fixture.Create<BookingLevelDto>();
    }

    protected BookingLevelDto[] getBookingLevelDtos()
    {
      return new[] { _levelDto1, _levelDto2, _levelDto3 };
    }
  }
}