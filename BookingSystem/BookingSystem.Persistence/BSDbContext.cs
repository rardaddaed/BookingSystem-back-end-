using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Persistence
{
  public class BSDbContext : DbContext
  {
    public BSDbContext(DbContextOptions<BSDbContext> options) : base(options)
    {
    }

    #region DbSets

    public virtual DbSet<BookingLevel> BookingLevels { get; set; }
    public virtual DbSet<BookingTeamArea> BookingTeamAreas { get; set; }
    public virtual DbSet<BookingObject> BookingObjects { get; set; }
    public virtual DbSet<BookingObjectType> BookingObjectTypes { get; set; }
    public virtual DbSet<BookingHistory> BookingHistories { get; set; }

    #endregion DbSets

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(typeof(BSDbContext).Assembly);
      foreach (var entity in modelBuilder.Model.GetEntityTypes())
      {
        entity.SetTableName(entity.DisplayName());
      }
    }
  }
}