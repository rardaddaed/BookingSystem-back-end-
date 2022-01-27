using BookingSystem.Domain.Models;

namespace BookingSystem.Repository
{
  public interface IBookingLevelRepository
  {
    Task CreateBookingLevel(CreateBookingLevelDto createBookingLevelDto, Guid bookingLevelId);
    Task DeleteBookingLevel(Guid bookingLevelId);
    Task<IEnumerable<BookingLevelDto>> GetAll();
    Task<BookingLevelDto?> GetByBookingLevelId(Guid bookingLevelId);
    Task UpdateBookingLevel(UpdateBookingLevelDto updateBookingLevelDto);
  }
}