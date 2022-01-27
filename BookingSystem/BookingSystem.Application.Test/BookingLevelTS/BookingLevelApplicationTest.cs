using AutoFixture;
using BookingSystem.Application.BookingLevelBL.Commands;
using BookingSystem.Application.BookingLevelBL.Queries;
using BookingSystem.Core.Exceptions;
using BookingSystem.Domain.Extensions;
using BookingSystem.Domain.Models;
using BookingSystem.Repository;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.TestHelper;

namespace BookingSystem.Application.Test.BookingLevelTS
{
  public class BookingLevelApplicationTest : BaseTestFixture
  {

    //[Test]
    //public async Task GetBookingLevelByIdTest()
    //{

    //  var handler = new GetBookingLevelByIdQueryHandler(_dbContext, _mapper);
    //  var result = await handler.Handle(new GetBookingLevelByIdQuery { BookingLevelId = _level1.BookingLevelId}, CancellationToken.None);

    //  result.ShouldBeOfType<BookingLevelDto>();
    //  result.ShouldBe(_level1.AdaptToDto());
    //}

    //[Test]
    //public async Task DeleteBookingLevelCommandTest()
    //{

    //  var handler = new DeleteBookingLevelCommandHandler(_dbContext);
    //  await handler.Handle(new DeleteBookingLevelCommand { BookingLevelId = _level1.BookingLevelId}, CancellationToken.None);

    //  var getHandler = new GetAllBookingLevelsQueryHandler(_dbContext, _mapper);
    //  var result = await getHandler.Handle(new GetAllBookingLevelsQuery(), CancellationToken.None);

    //  result.Count().ShouldBe(getBookingLevels().Length - 1);

    //}

    [Test]
    public async Task GetAllBookingLevelsTest()
    {
      var mockBookingLevelRepository = new Mock<IBookingLevelRepository>();

      mockBookingLevelRepository.Setup(x => x.GetAll())
        .Returns(fixture.Create<Task<IEnumerable<BookingLevelDto>>>());

      var handler = new GetAllBookingLevelsQueryHandler(mockBookingLevelRepository.Object);
      var result = await handler.Handle(new GetAllBookingLevelsQuery(), CancellationToken.None);

      mockBookingLevelRepository.Verify(x => x.GetAll(), Times.Once());
    }

    [Test]
    public async Task GetBookingLevelByIdTest()
    {
      var mockBookingLevelRepository = new Mock<IBookingLevelRepository>();

      mockBookingLevelRepository.Setup(x => x.GetByBookingLevelId(It.IsAny<Guid>()))
        .Returns(fixture.Create<Task<BookingLevelDto>>());

      var handler = new GetBookingLevelByIdQueryHandler(mockBookingLevelRepository.Object);
      var result = await handler.Handle(fixture.Create<GetBookingLevelByIdQuery>(), CancellationToken.None);

      mockBookingLevelRepository.Verify(x => x.GetByBookingLevelId(It.IsAny<Guid>()), Times.Once());
    }

    [Test]
    public async Task CreateBookingLevelCommandTest()
    {
      var mockBookingLevelRepository = new Mock<IBookingLevelRepository>();

      mockBookingLevelRepository.Setup(x => x.CreateBookingLevel(It.IsAny<CreateBookingLevelDto>(), It.IsAny<Guid>()))
        .Returns(fixture.Create<Task>());
      mockBookingLevelRepository.Setup(x => x.GetByBookingLevelId(It.IsAny<Guid>()))
        .Returns(fixture.Create<Task<BookingLevelDto>>());

      var handler = new CreateBookingLevelCommandHandler(mockBookingLevelRepository.Object);
      var result = await handler.Handle(fixture.Create<CreateBookingLevelCommand>(), CancellationToken.None);

      mockBookingLevelRepository.Verify(x => x.GetByBookingLevelId(It.IsAny<Guid>()), Times.Once());
      mockBookingLevelRepository.Verify(x => x.CreateBookingLevel(It.IsAny<CreateBookingLevelDto>(), It.IsAny<Guid>()), Times.Once());
    }

    [Test]
    public void CreateBookingLevelCommandValidatorTest()
    {
      var validator = new CreateBookingLevelCommandValidator();

      var bookingLevelCommand = new CreateBookingLevelCommand(new CreateBookingLevelDto
      {
        Name = "Name",
        Alias = "Alias",
        BlueprintUrl = "Url",
        BlueprintWidth = 3,
        BlueprintHeight = 3,
        MaxBooking = 3,
        Locked = true
      });

      var result = validator.TestValidate(bookingLevelCommand);
      result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void CreateBookingLevelCommandValidatorShouldHaveErrorsTest()
    {
      var validator = new CreateBookingLevelCommandValidator();

      var bookingLevelCommand = new CreateBookingLevelCommand(new CreateBookingLevelDto
      {
        Name = "",
        Alias = "sldkjflkasdglsdnlsdfkjslkdjlkwboebwogodslbflkxnslkcvnmlxkcnvlsndofknxlckvnlxnldkjflksfowhefijosdnvlknxclknpajspdjqpwjdonvdosjbneofhobnfgdbnfgknsdfjslkdfjaslkfjaklfjsdlkhnsldvbsldnvlsdk",
        BlueprintUrl = "Url",
        BlueprintWidth = 3,
        BlueprintHeight = 3,
        MaxBooking = -3,
        Locked = true
      });

      var result = validator.TestValidate(bookingLevelCommand);
      result.ShouldHaveValidationErrorFor(x => x.CreateBookingLevelDto.Name);
      result.ShouldHaveValidationErrorFor(x => x.CreateBookingLevelDto.Alias);
      result.ShouldHaveValidationErrorFor(x => x.CreateBookingLevelDto.MaxBooking);
    }

    [Test]
    public async Task UpdateBookingLevelCommandTest()
    {
      var mockBookingLevelRepository = new Mock<IBookingLevelRepository>();

      mockBookingLevelRepository.Setup(x => x.GetByBookingLevelId(It.IsAny<Guid>()))
        .Returns(fixture.Create<Task<BookingLevelDto>>());
      mockBookingLevelRepository.Setup(x => x.UpdateBookingLevel(It.IsAny<UpdateBookingLevelDto>()))
        .Returns(fixture.Create<Task>());

      var handler = new UpdateBookingLevelCommandHandler(mockBookingLevelRepository.Object);
      var result = await handler.Handle(fixture.Create<UpdateBookingLevelCommand>(), CancellationToken.None);

      mockBookingLevelRepository.Verify(x => x.GetByBookingLevelId(It.IsAny<Guid>()), Times.Exactly(2));
      mockBookingLevelRepository.Verify(x => x.UpdateBookingLevel(It.IsAny<UpdateBookingLevelDto>()), Times.Once());

    }

    [Test]
    public void UpdateBookingLevelCommandTestWithException()
    {
      var mockBookingLevelRepository = new Mock<IBookingLevelRepository>();

      mockBookingLevelRepository.Setup(x => x.GetByBookingLevelId(It.IsAny<Guid>()))
        .Returns(Task.FromResult<BookingLevelDto>(null));

      var handler = new UpdateBookingLevelCommandHandler(mockBookingLevelRepository.Object);
      Should.Throw<BookingSystemException<Guid>>(async () => await handler.Handle(fixture
        .Create<UpdateBookingLevelCommand>(), CancellationToken.None))
        .Message.ShouldBe("Booking level not found");

      mockBookingLevelRepository.Verify(x => x.GetByBookingLevelId(It.IsAny<Guid>()), Times.Once());
    }


    [Test]
    public void UpdateBookingLevelCommandValidatorTest()
    {
      var validator = new UpdateBookingLevelCommandValidator();

      var bookingLevelCommand = new UpdateBookingLevelCommand(new UpdateBookingLevelDto
      {
        Name = "Name",
        Alias = "Alias",
        BlueprintUrl = "Url",
        BlueprintWidth = 3,
        BlueprintHeight = 3,
        MaxBooking = 3,
        Locked = true
      });

      var result = validator.TestValidate(bookingLevelCommand);
      result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void UpdateBookingLevelCommandValidatorShouldHaveErrorsTest()
    {
      var validator = new UpdateBookingLevelCommandValidator();

      var bookingLevelCommand = new UpdateBookingLevelCommand(new UpdateBookingLevelDto
      {
        Name = "",
        Alias = "sldkjflkasdglsdnlsdfkjslkdjlkwboebwogodslbflkxnslkcvnmlxkcnvlsndofknxlckvnlxnldkjflksfowhefijosdnvlknxclknpajspdjqpwjdonvdosjbneofhobnfgdbnfgknsdfjslkdfjaslkfjaklfjsdlkhnsldvbsldnvlsdk",
        BlueprintUrl = "Url",
        BlueprintWidth = 3,
        BlueprintHeight = 3,
        MaxBooking = -3,
        Locked = true
      });

      var result = validator.TestValidate(bookingLevelCommand);
      result.ShouldHaveValidationErrorFor(x => x.UpdateBookingLevelDto.Name);
      result.ShouldHaveValidationErrorFor(x => x.UpdateBookingLevelDto.Alias);
      result.ShouldHaveValidationErrorFor(x => x.UpdateBookingLevelDto.MaxBooking);
    }

    [Test]
    public async Task DeleteBookingLevelCommandTest()
    {
      var mockBookingLevelRepository = new Mock<IBookingLevelRepository>();

      mockBookingLevelRepository.Setup(x => x.GetByBookingLevelId(It.IsAny<Guid>()))
        .Returns(fixture.Create<Task<BookingLevelDto>>());
      mockBookingLevelRepository.Setup(x => x.DeleteBookingLevel(It.IsAny<Guid>()))
        .Returns(fixture.Create<Task>());

      var handler = new DeleteBookingLevelCommandHandler(mockBookingLevelRepository.Object);
      var result = await handler.Handle(fixture.Create<DeleteBookingLevelCommand>(), CancellationToken.None);

      mockBookingLevelRepository.Verify(x => x.GetByBookingLevelId(It.IsAny<Guid>()), Times.Once());
      mockBookingLevelRepository.Verify(x => x.DeleteBookingLevel(It.IsAny<Guid>()), Times.Once());
    }

    [Test]
    public void DeleteBookingLevelCommandTestWithException()
    {
      var mockBookingLevelRepository = new Mock<IBookingLevelRepository>();

      mockBookingLevelRepository.Setup(x => x.GetByBookingLevelId(It.IsAny<Guid>()))
        .Returns(Task.FromResult<BookingLevelDto>(null));

      var handler = new DeleteBookingLevelCommandHandler(mockBookingLevelRepository.Object);
      Should.Throw<BookingSystemException<Guid>>(async () => await handler.Handle(fixture
        .Create<DeleteBookingLevelCommand>(), CancellationToken.None))
        .Message.ShouldBe("Booking level not found");

      mockBookingLevelRepository.Verify(x => x.GetByBookingLevelId(It.IsAny<Guid>()), Times.Once());
    }
  }
}