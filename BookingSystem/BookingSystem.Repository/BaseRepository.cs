using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingSystem.Persistence;

namespace BookingSystem.Repository
{
  public abstract class BaseRepository
  {
    protected readonly BSDbContext _dbContext;
    public BaseRepository(BSDbContext dbContext)
    {
      _dbContext = dbContext;
    }
  }
}
