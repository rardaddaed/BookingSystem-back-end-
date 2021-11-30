using System.Threading.Tasks;

namespace BookingSystem.Persistence.Seeders
{
  public class SeederLocal : BaseSeeder
  {
    public SeederLocal(BSDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task Seed()
    {
      await base.Seed();
    }
  }
}