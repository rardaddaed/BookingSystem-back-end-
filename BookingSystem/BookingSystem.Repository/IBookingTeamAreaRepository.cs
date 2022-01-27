using BookingSystem.Domain.Models;

namespace BookingSystem.Repository
{
  public interface IBookingTeamAreaRepository
  {
    Task CreateBookingTeamArea(CreateBookingTeamAreaDto createBookingTeamAreaDto, Guid bookingTeamAreaId);
    Task DeleteBookingTeamArea(Guid bookingTeamAreaId);
    Task<IEnumerable<BookingTeamAreaDto>> GetAll();
    Task<BookingTeamAreaDto?> GetByBookingTeamAreaId(Guid bookingTeamAreaId);
    Task UpdateBookingTeamArea(UpdateBookingTeamAreaDto updateBookingTeamAreaDto);
  }
}