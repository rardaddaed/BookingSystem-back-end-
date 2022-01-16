using BookingSystem.Application.BookingLevelBL.Commands;
using BookingSystem.Application.BookingLevelBL.Queries;
using BookingSystem.Domain.Extensions;
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

      var handler = new GetAllBookingLevelsQueryHandler(_dbContext, _mapper);
      var result = await handler.Handle(new GetAllBookingLevelsQuery(), CancellationToken.None);

      result.Count().ShouldBe(getBookingLevels().Length);
      result.ShouldBeOfType<BookingLevelDto[]>();
    }

    [Test]
    public async Task GetBookingLevelByIdTest()
    {

      var handler = new GetBookingLevelByIdQueryHandler(_dbContext, _mapper);
      var result = await handler.Handle(new GetBookingLevelByIdQuery { BookingLevelId = _level1.BookingLevelId}, CancellationToken.None);

      result.ShouldBeOfType<BookingLevelDto>();
      result.ShouldBe(_level1.AdaptToDto());
    }

    [Test]
    public async Task DeleteBookingLevelCommandTest()
    {

      var handler = new DeleteBookingLevelCommandHandler(_dbContext);
      await handler.Handle(new DeleteBookingLevelCommand { BookingLevelId = _level1.BookingLevelId}, CancellationToken.None);

      var getHandler = new GetAllBookingLevelsQueryHandler(_dbContext, _mapper);
      var result = await getHandler.Handle(new GetAllBookingLevelsQuery(), CancellationToken.None);

      result.Count().ShouldBe(getBookingLevels().Length - 1);

    }

    [Test]
    public async Task UpdateBookingLevelCommandTest() {

      var handler = new UpdateBookingLevelCommandHandler(_dbContext, _mapper);
      var result = await handler.Handle(new UpdateBookingLevelCommand(
        new UpdateBookingLevelDto
        {
          bookingLevelId = _level1.BookingLevelId,
          Name = "Test",
          Alias = "T",
          BlueprintUrl = "Url",
          BlueprintWidth = 1,
          BlueprintHeight = 1,
          MaxBooking = 2,
          Locked = true
        })
      , CancellationToken.None);

      result.ShouldBeOfType<BookingLevelDto>();
      result.Name.ShouldBe("Test");
      result.Alias.ShouldBe("T");
      result.BlueprintUrl.ShouldBe("Url");
      result.BlueprintWidth.ShouldBe(1);
      result.BlueprintHeight.ShouldBe(1);
      result.MaxBooking.ShouldBe(2);
      result.Locked.ShouldBe(true);
    }

    [Test]
    public async Task CreateBookingLevelCommandTest()
    {

      var handler = new CreateBookingLevelCommandHandler(_dbContext, _mapper);
      var result = await handler.Handle(new CreateBookingLevelCommand(new CreateBookingLevelDto
      {
        Name = "Test",
        Alias = "T",
        BlueprintUrl = "Url",
        BlueprintWidth = 1,
        BlueprintHeight = 1,
        MaxBooking = 2,
        Locked = true
      }), CancellationToken.None);

      result.ShouldBeOfType<BookingLevelDto>();
      result.Name.ShouldBe("Test");
      result.Alias.ShouldBe("T");
      result.BlueprintUrl.ShouldBe("Url");
      result.BlueprintWidth.ShouldBe(1);
      result.BlueprintHeight.ShouldBe(1);
      result.MaxBooking.ShouldBe(2);
      result.Locked.ShouldBe(true);

      var getHandler = new GetAllBookingLevelsQueryHandler(_dbContext, _mapper);
      var getResult = await getHandler.Handle(new GetAllBookingLevelsQuery(), CancellationToken.None);

      getResult.Count().ShouldBe(getBookingLevels().Length + 1);
    }
  }
}