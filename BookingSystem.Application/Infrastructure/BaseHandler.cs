using BookingSystem.Persistence;

namespace BookingSystem.Application.Infrastructure
{
  public abstract class BaseHandler
  {
    protected readonly BSDbContext _dbContext;

    public BaseHandler(BSDbContext dbContext)
    {
      _dbContext = dbContext;
    }
  }
}