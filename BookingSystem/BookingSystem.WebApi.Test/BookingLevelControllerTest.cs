using BookingSystem.Application.BookingLevelBL.Queries;
using BookingSystem.Domain.Models;
using MediatR;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookingSystem.WebApi.Test
{
  public class BookingLevelControllerTest : BaseTestFixture
  {
    [Test]
    public async Task GetAllBookingLevelsTest()
    {
      var mediator = new Mock<IMediator>();
      mediator.Setup(m => m.Send(It.IsAny<GetAllBookingLevelsQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(getBookingLevelDtos());

      var result = await mediator.Object.Send(new GetAllBookingLevelsQuery());

      mediator.Verify(x => x.Send(It.IsAny<GetAllBookingLevelsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
      result.Count().ShouldBe(getBookingLevelDtos().Length);
      result.ShouldBeOfType<BookingLevelDto[]>();
    }
  }
}