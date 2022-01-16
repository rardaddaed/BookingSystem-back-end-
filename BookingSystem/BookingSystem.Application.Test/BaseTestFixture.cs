using AutoFixture;
using AutoMapper;
using BookingSystem.Domain;
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
    protected IMapper _mapper;

    protected BookingLevel _level1;
    protected BookingLevel _level2;
    protected BookingLevel _level3;

    protected BookingTeamArea _teamArea1;
    protected BookingTeamArea _teamArea2;
    protected BookingTeamArea _teamArea3;

    [SetUp]
    public async Task SetUpDbContext()
    {
      var fixture = new Fixture();
      fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
      fixture.Behaviors.Add(new OmitOnRecursionBehavior());

      _level1 = fixture.Create<BookingLevel>();
      _level2 = fixture.Create<BookingLevel>();
      _level3 = fixture.Create<BookingLevel>();

      _teamArea1 = fixture.Create<BookingTeamArea>();
      _teamArea1.BookingLevelId = _level1.BookingLevelId;
      _teamArea2 = fixture.Create<BookingTeamArea>();
      _teamArea2.BookingLevelId = _level1.BookingLevelId;
      _teamArea3 = fixture.Create<BookingTeamArea>();
      _teamArea3.BookingLevelId = _level1.BookingLevelId;

      _level1.BookingTeamAreas.Add(_teamArea1);
      _level1.BookingTeamAreas.Add(_teamArea2);
      _level1.BookingTeamAreas.Add(_teamArea3);

      var options = new DbContextOptionsBuilder<BSDbContext>()
        .UseInMemoryDatabase("Test")
        .Options;
      _dbContext = new BSDbContext(options);

      await _dbContext.BookingLevels.AddRangeAsync(getBookingLevels());

      await _dbContext.SaveChangesAsync();

      var mappingConfiguration = new MapperConfiguration(cfg =>
      {
        cfg.AddProfile<MappingProfile>();
      });
      _mapper = mappingConfiguration.CreateMapper();
    }

    [TearDown]
    public async Task DeleteInMemoryDatabase()
    {
      await _dbContext.Database.EnsureDeletedAsync();
    }

    protected BookingLevel[] getBookingLevels()
    {
      return new[] { _level1, _level2, _level3 };
    }

    protected BookingTeamArea[] getBookingTeamAreas()
    {
      return new[] { _teamArea1, _teamArea2, _teamArea3 };
    }

    public ValueTask DisposeAsync()
    {
      return _dbContext.DisposeAsync();
    }
  }
}