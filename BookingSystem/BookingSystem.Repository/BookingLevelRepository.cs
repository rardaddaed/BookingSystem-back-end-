using BookingSystem.Domain.Models;
using BookingSystem.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingSystem.Core.Exceptions;

namespace BookingSystem.Repository
{
  public class BookingLevelRepository : BaseRepository, IBookingLevelRepository
  {
    public BookingLevelRepository(BSDbContext dbContext) : base(dbContext) { }

    public async Task<IEnumerable<BookingLevelDto>> GetAll()
    {
      var sql = @"SELECT [BookingLevelId]
      ,[Name]
      ,[Alias]
      ,[BlueprintUrl]
      ,[BlueprintWidth]
      ,[BlueprintHeight]
      ,[MaxBooking]
      ,[Locked]
      ,[CreatedUtc]
      ,[ModifiedUtc]
       FROM [dbo].[BookingLevel]";

      var bookingLevels = await _dbContext.Database.GetDbConnection().QueryAsync<BookingLevelDto>(sql);

      return bookingLevels;
    }

    public async Task<BookingLevelDto?> GetByBookingLevelId(Guid bookingLevelId)
    {
      var sql = @"SELECT [BookingLevelId]
      ,[Name]
      ,[Alias]
      ,[BlueprintUrl]
      ,[BlueprintWidth]
      ,[BlueprintHeight]
      ,[MaxBooking]
      ,[Locked]
      ,[CreatedUtc]
      ,[ModifiedUtc]
       FROM [dbo].[BookingLevel] WHERE BookingLevelId = @BookingLevelId;";

      return (await _dbContext.Database.GetDbConnection()
        .QueryAsync<BookingLevelDto>(sql, new { BookingLevelId = bookingLevelId }))
        .FirstOrDefault();
    }

    public async Task CreateBookingLevel(CreateBookingLevelDto createBookingLevelDto, Guid bookingLevelId)
    {

      var sql = @"INSERT dbo.BookingLevel(BookingLevelId, Name, Alias, BlueprintUrl, BlueprintWidth, BlueprintHeight, Maxbooking, Locked)  
                  VALUES(@BookingLevelId, @Name, @Alias, @Url, @Width, @Height, @Maxbooking, @Locked)";

      await _dbContext.Database.GetDbConnection()
        .ExecuteAsync(sql, new
        {
          BookingLevelId = bookingLevelId,
          Name = createBookingLevelDto.Name,
          Alias = createBookingLevelDto.Alias,
          Url = createBookingLevelDto.BlueprintUrl,
          Width = createBookingLevelDto.BlueprintWidth,
          Height = createBookingLevelDto.BlueprintHeight,
          Maxbooking = createBookingLevelDto.MaxBooking,
          Locked = createBookingLevelDto.Locked
        });
    }

    public async Task UpdateBookingLevel(UpdateBookingLevelDto updateBookingLevelDto)
    {
      var sql = @"UPDATE BookingLevel
                  SET Name = @Name, Alias = @Alias, BlueprintUrl = @BlueprintUrl, BlueprintWidth = @BlueprintWidth, BlueprintHeight = @BlueprintHeight, Maxbooking = @Maxbooking, Locked = @Locked 
                  WHERE BookingLevelId = @BookingLevelId;";

      await _dbContext.Database.GetDbConnection()
         .ExecuteAsync(sql, new
         {
           updateBookingLevelDto.Name,
           updateBookingLevelDto.Alias,
           updateBookingLevelDto.BlueprintUrl,
           updateBookingLevelDto.BlueprintWidth,
           updateBookingLevelDto.BlueprintHeight,
           updateBookingLevelDto.MaxBooking,
           updateBookingLevelDto.Locked,
           updateBookingLevelDto.BookingLevelId
         });
    }

    public async Task DeleteBookingLevel(Guid bookingLevelId)
    {
      var sql = "DELETE FROM BookingLevel WHERE BookingLevelId = @Id";

      await _dbContext.Database.GetDbConnection()
        .ExecuteAsync(sql, new { Id = bookingLevelId });
    }
  }
}
