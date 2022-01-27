using BookingSystem.Core.Exceptions;
using BookingSystem.Domain.Models;
using BookingSystem.Persistence;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Repository
{
  public class BookingTeamAreaRepository : BaseRepository, IBookingTeamAreaRepository
  {
    public BookingTeamAreaRepository(BSDbContext dbContext) : base(dbContext) { }

    public async Task<IEnumerable<BookingTeamAreaDto>> GetAll()
    {
      var sql = @"SELECT [BookingTeamAreaId]
      ,[BookingLevelId]
      ,[Name]
      ,[Coords]
      ,[Locked]
      ,[CreatedUtc]
      ,[ModifiedUtc]
       FROM [dbo].[BookingTeamArea]";

      var bookingTeamAreas = await _dbContext.Database.GetDbConnection().QueryAsync<BookingTeamAreaDto>(sql);

      return bookingTeamAreas;
    }

    public async Task<BookingTeamAreaDto?> GetByBookingTeamAreaId(Guid bookingTeamAreaId)
    {
      var sql = @"SELECT [BookingTeamAreaId]
      ,[BookingLevelId]
      ,[Name]
      ,[Coords]
      ,[Locked]
      ,[CreatedUtc]
      ,[ModifiedUtc]
       FROM [dbo].[BookingTeamArea] WHERE BookingTeamAreaId = @BookingTeamAreaId";

      var bookingTeamArea = (await _dbContext.Database.GetDbConnection()
        .QueryAsync<BookingTeamAreaDto>(sql, new { BookingTeamAreaId = bookingTeamAreaId }))
        .FirstOrDefault();

      return bookingTeamArea;
    }

    public async Task CreateBookingTeamArea(CreateBookingTeamAreaDto createBookingTeamAreaDto, Guid bookingTeamAreaId)
    {
      var sql = @"INSERT dbo.BookingTeamArea(BookingTeamAreaId, BookingLevelId, Name, Coords, Locked)
                  VALUES(@BookingTeamAreaId, @BookingLevelId, @Name, @Coords, @Locked)";

      await _dbContext.Database.GetDbConnection()
        .ExecuteAsync(sql, new
        {
          BookingTeamAreaId = bookingTeamAreaId,
          BookingLevelId = createBookingTeamAreaDto.BookingLevelId,
          Name = createBookingTeamAreaDto.Name,
          Coords = createBookingTeamAreaDto.Coords,
          Locked = createBookingTeamAreaDto.Locked
        });
    }

    public async Task UpdateBookingTeamArea(UpdateBookingTeamAreaDto updateBookingTeamAreaDto)
    {
      var sql = @"UPDATE BookingTeamArea
                  SET BookingLevelId = @BookingLevelId, Name = @Name, Coords = @Coords, @Locked = Locked
                  WHERE BookingTeamAreaId = @BookingTeamAreaId;";

      await _dbContext.Database.GetDbConnection()
        .ExecuteAsync(sql, new
        {
          updateBookingTeamAreaDto.BookingLevelId,
          updateBookingTeamAreaDto.Name,
          updateBookingTeamAreaDto.Coords,
          updateBookingTeamAreaDto.Locked,
          updateBookingTeamAreaDto.BookingTeamAreaId
        });
    }

    public async Task DeleteBookingTeamArea(Guid bookingTeamAreaId)
    {
      var sql = "DELETE FROM BookingTeamArea WHERE BookingTeamAreaId = @Id";

      await _dbContext.Database.GetDbConnection()
        .ExecuteAsync(sql, new { Id = bookingTeamAreaId });
    }
  }
}
