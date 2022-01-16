using BookingSystem.Application.BookingTeamAreaBL.Commands;
using BookingSystem.Application.BookingTeamAreaBL.Queries;
using BookingSystem.Domain.Extensions;
using BookingSystem.Domain.Models;
using NUnit.Framework;
using Shouldly;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace BookingSystem.Application.Test.BookingTeamAreaTS
{
  public class BookingTeamAreaApplicationTest :BaseTestFixture
  {
    [Test]
    public async Task GetAllBookingTeamAreasTest()
    {

      var handler = new GetAllBookingTeamAreasQueryHandler(_dbContext);
      var result = await handler.Handle(new GetAllBookingTeamAreasQuery(), CancellationToken.None);

      result.Count().ShouldBe(getBookingTeamAreas().Length);
      result.ShouldBeOfType<BookingTeamAreaDto[]>();
    }

    [Test]
    public async Task GetBookingTeamAreaByIdTest()
    {

      var handler = new GetBookingTeamAreaByIdQueryHandler(_dbContext);
      var result = await handler.Handle(new GetBookingTeamAreaByIdQuery { BookingTeamAreaId = _teamArea1.BookingTeamAreaId}, CancellationToken.None);

      result.ShouldBeOfType<BookingTeamAreaDto>();

    }

    [Test]
    public async Task DeleteBookingTeamAreaCommandTest()
    {

      var handler = new DeleteBookingTeamAreaCommandHandler(_dbContext);
      await handler.Handle(new DeleteBookingTeamAreaCommand { BookingTeamAreaId = _teamArea1.BookingTeamAreaId }, CancellationToken.None);

      var getHandler = new GetAllBookingTeamAreasQueryHandler(_dbContext);
      var result = await getHandler.Handle(new GetAllBookingTeamAreasQuery(), CancellationToken.None);

      result.Count().ShouldBe(getBookingTeamAreas().Length - 1);

    }

    [Test]
    public async Task UpdateBookingTeamAreaCommandTest()
    {

      var handler = new UpdateBookingTeamAreaCommandHandler(_dbContext);
      var result = await handler.Handle(new UpdateBookingTeamAreaCommand
      {
        BookingTeamAreaId = _teamArea1.BookingTeamAreaId,
        Name = "Test",
        Coords = "1",
        Locked = true

      }, CancellationToken.None);

      result.ShouldBeOfType<BookingTeamAreaDto>();
      result.Name.ShouldBe("Test");
      result.Coords.ShouldBe("1");
      result.Locked.ShouldBe(true);
    }

    [Test]
    public async Task CreateBookingTeamAreaCommandTest()
    {

      var handler = new CreateBookingTeamAreaCommandHandler(_dbContext);
      var result = await handler.Handle(new CreateBookingTeamAreaCommand
      {
        Name = "Test",
        Coords= "1",
        Locked = true
      }, CancellationToken.None);

      result.ShouldBeOfType<BookingTeamAreaDto>();
      result.Name.ShouldBe("Test");
      result.Coords.ShouldBe("1");
      result.Locked.ShouldBe(true);

      var getHandler = new GetAllBookingTeamAreasQueryHandler(_dbContext);
      var getResult = await getHandler.Handle(new GetAllBookingTeamAreasQuery(), CancellationToken.None);

      getResult.Count().ShouldBe(getBookingTeamAreas().Length + 1);

    }

  }
}
