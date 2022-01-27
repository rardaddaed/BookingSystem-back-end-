using AutoFixture;
using BookingSystem.Application.BookingTeamAreaBL.Commands;
using BookingSystem.Application.BookingTeamAreaBL.Queries;
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


namespace BookingSystem.Application.Test.BookingTeamAreaTS
{
  public class BookingTeamAreaApplicationTest :BaseTestFixture
  {
    [Test]
    public async Task GetAllBookingTeamAreasTest()
    {
      var mockBookingLevelRepository = new Mock<IBookingTeamAreaRepository>();

      mockBookingLevelRepository.Setup(x => x.GetAll())
        .Returns(fixture.Create<Task<IEnumerable<BookingTeamAreaDto>>>());

      var handler = new GetAllBookingTeamAreasQueryHandler(mockBookingLevelRepository.Object);
      var result = await handler.Handle(new GetAllBookingTeamAreasQuery(), CancellationToken.None);

      mockBookingLevelRepository.Verify(x => x.GetAll(), Times.Once());
    }

    [Test]
    public async Task GetBookingTeamAreaByIdTest()
    {
      var mockBookingLevelRepository = new Mock<IBookingTeamAreaRepository>();

      mockBookingLevelRepository.Setup(x => x.GetByBookingTeamAreaId(It.IsAny<Guid>()))
        .Returns(fixture.Create<Task<BookingTeamAreaDto>>());

      var handler = new GetBookingTeamAreaByIdQueryHandler(mockBookingLevelRepository.Object);
      var result = await handler.Handle(new GetBookingTeamAreaByIdQuery(), CancellationToken.None);

      mockBookingLevelRepository.Verify(x => x.GetByBookingTeamAreaId(It.IsAny<Guid>()), Times.Once());
    }

    [Test]
    public async Task CreateBookingTeamAreaCommandTest()
    {
      var mockBookingLevelRepository = new Mock<IBookingTeamAreaRepository>();

      mockBookingLevelRepository.Setup(x => x.CreateBookingTeamArea(It.IsAny<CreateBookingTeamAreaDto>(), It.IsAny<Guid>()))
        .Returns(fixture.Create<Task>());
      mockBookingLevelRepository.Setup(x => x.GetByBookingTeamAreaId(It.IsAny<Guid>()))
        .Returns(fixture.Create<Task<BookingTeamAreaDto>>());

      var handler = new CreateBookingTeamAreaCommandHandler(mockBookingLevelRepository.Object);
      var result = await handler.Handle(fixture.Create<CreateBookingTeamAreaCommand>(), CancellationToken.None);

      mockBookingLevelRepository.Verify(x => x.GetByBookingTeamAreaId(It.IsAny<Guid>()), Times.Once());
      mockBookingLevelRepository.Verify(x => x.CreateBookingTeamArea(It.IsAny<CreateBookingTeamAreaDto>(), It.IsAny<Guid>()), Times.Once());

    }

    [Test]
    public void CreateBookingTeamAreaCommandValidatorTest()
    {
      var validator = new CreateBookingTeamAreaCommandValidator();

      var bookingTeamAreaCommand = new CreateBookingTeamAreaCommand(new CreateBookingTeamAreaDto
      {
        BookingLevelId = Guid.NewGuid(),
        Name = "Name",
        Coords = "Coords",
        Locked = true
      });

      var result = validator.TestValidate(bookingTeamAreaCommand);
      result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void CreateBookingTeamAreaCommandValidatorShouldHaveErrorsTest()
    {
      var validator = new CreateBookingTeamAreaCommandValidator();

      var bookingTeamAreaCommand = new CreateBookingTeamAreaCommand(new CreateBookingTeamAreaDto
      {
        BookingLevelId = Guid.Empty,
        Name = "",
        Coords = "Coords",
        Locked = true
      });

      var result = validator.TestValidate(bookingTeamAreaCommand);
      result.ShouldHaveValidationErrorFor(x => x.CreateBookingTeamAreaDto.BookingLevelId);
      result.ShouldHaveValidationErrorFor(x => x.CreateBookingTeamAreaDto.Name);
    }

    [Test]
    public async Task UpdateBookingTeamAreaCommandTest()
    {
      var mockBookingLevelRepository = new Mock<IBookingTeamAreaRepository>();

      mockBookingLevelRepository.Setup(x => x.GetByBookingTeamAreaId(It.IsAny<Guid>()))
        .Returns(fixture.Create<Task<BookingTeamAreaDto>>());
      mockBookingLevelRepository.Setup(x => x.UpdateBookingTeamArea(It.IsAny<UpdateBookingTeamAreaDto>()))
        .Returns(fixture.Create<Task>());

      var handler = new UpdateBookingTeamAreaCommandHandler(mockBookingLevelRepository.Object);
      var result = await handler.Handle(fixture.Create<UpdateBookingTeamAreaCommand>(), CancellationToken.None);

      mockBookingLevelRepository.Verify(x => x.GetByBookingTeamAreaId(It.IsAny<Guid>()), Times.Exactly(2));
      mockBookingLevelRepository.Verify(x => x.UpdateBookingTeamArea(It.IsAny<UpdateBookingTeamAreaDto>()), Times.Once());

    }

    [Test]
    public void UpdateBookingTeamAreaCommandTestWithException()
    {
      var mockBookingLevelRepository = new Mock<IBookingTeamAreaRepository>();

      mockBookingLevelRepository.Setup(x => x.GetByBookingTeamAreaId(It.IsAny<Guid>()))
        .Returns(Task.FromResult<BookingTeamAreaDto>(null));

      var handler = new UpdateBookingTeamAreaCommandHandler(mockBookingLevelRepository.Object);
      Should.Throw<BookingSystemException<Guid>>(async () => await handler.Handle(fixture
        .Create<UpdateBookingTeamAreaCommand>(), CancellationToken.None))
        .Message.ShouldBe("Booking team area not found");

      mockBookingLevelRepository.Verify(x => x.GetByBookingTeamAreaId(It.IsAny<Guid>()), Times.Once());
    }

    [Test]
    public void UpdateBookingTeamAreaCommandValidatorTest()
    {
      var validator = new UpdateBookingTeamAreaCommandValidator();

      var bookingTeamAreaCommand = new UpdateBookingTeamAreaCommand(new UpdateBookingTeamAreaDto
      {
        BookingLevelId = Guid.NewGuid(),
        Name = "Name",
        Coords = "Coords",
        Locked = true
      });

      var result = validator.TestValidate(bookingTeamAreaCommand);
      result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void UpdateBookingTeamAreaCommandValidatorShouldHaveErrorsTest()
    {
      var validator = new UpdateBookingTeamAreaCommandValidator();

      var bookingTeamAreaCommand = new UpdateBookingTeamAreaCommand(new UpdateBookingTeamAreaDto
      {
        BookingLevelId = Guid.Empty,
        Name = "",
        Coords = "Coords",
        Locked = true
      });

      var result = validator.TestValidate(bookingTeamAreaCommand);
      result.ShouldHaveValidationErrorFor(x => x.UpdateBookingTeamAreaDto.BookingLevelId);
      result.ShouldHaveValidationErrorFor(x => x.UpdateBookingTeamAreaDto.Name);
    }

    [Test]
    public async Task DeleteBookingLevelCommandTest()
    {
      var mockBookingLevelRepository = new Mock<IBookingTeamAreaRepository>();

      mockBookingLevelRepository.Setup(x => x.GetByBookingTeamAreaId(It.IsAny<Guid>()))
         .Returns(fixture.Create<Task<BookingTeamAreaDto>>());
      mockBookingLevelRepository.Setup(x => x.DeleteBookingTeamArea(It.IsAny<Guid>()))
        .Returns(fixture.Create<Task>());

      var handler = new DeleteBookingTeamAreaCommandHandler(mockBookingLevelRepository.Object);
      var result = await handler.Handle(fixture.Create<DeleteBookingTeamAreaCommand>(), CancellationToken.None);

      mockBookingLevelRepository.Verify(x => x.GetByBookingTeamAreaId(It.IsAny<Guid>()), Times.Once());
      mockBookingLevelRepository.Verify(x => x.DeleteBookingTeamArea(It.IsAny<Guid>()), Times.Once());
    }

    [Test]
    public void DeleteBookingLevelCommandTestWithException()
    {
      var mockBookingLevelRepository = new Mock<IBookingTeamAreaRepository>();

      mockBookingLevelRepository.Setup(x => x.GetByBookingTeamAreaId(It.IsAny<Guid>()))
        .Returns(Task.FromResult<BookingTeamAreaDto>(null));

      var handler = new DeleteBookingTeamAreaCommandHandler(mockBookingLevelRepository.Object);
      Should.Throw<BookingSystemException<Guid>>(async () => await handler.Handle(fixture
        .Create<DeleteBookingTeamAreaCommand>(), CancellationToken.None))
        .Message.ShouldBe("Booking team area not found");

      mockBookingLevelRepository.Verify(x => x.GetByBookingTeamAreaId(It.IsAny<Guid>()), Times.Once());
    }
   
  }
}
