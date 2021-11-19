using BookingSystem.Application.BookingLevelBL.Queries;
using BookingSystem.Domain.Models;
using NUnit.Framework;
using Shouldly;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookingSystem.Application.Test.BookingLevelTS
{
  public class BookingLevelApplicationTest : BaseTestFixture
  {
    [Test]
    public async Task GetAllBookingLevelsTest()
    {
      var handler = new GetAllBookingLevelsQueryHandler(_dbContext);
      var result = await handler.Handle(new GetAllBookingLevelsQuery(), CancellationToken.None);

      result.Count().ShouldBe(getBookingLevels().Length);
      result.ShouldBeOfType<BookingLevelDto[]>();
    }
  }
}