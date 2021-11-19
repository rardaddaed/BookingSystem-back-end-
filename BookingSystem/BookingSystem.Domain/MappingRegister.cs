using BookingSystem.Domain.Entities;
using Mapster;

namespace BookingSystem.Domain
{
  public class MappingRegister : ICodeGenerationRegister
  {
    public void Register(CodeGenerationConfig config)
    {
      config.AdaptTo("[name]Dto", MapType.Map | MapType.MapToTarget | MapType.Projection)
        .ForType<BookingLevel>(cfg =>
        {
          cfg.Ignore(it => it.BookingObjects);
          cfg.Ignore(it => it.BookingTeamAreas);
        });

      config.GenerateMapper("[name]Mapper")
        .ForType<BookingLevel>();
    }
  }
}