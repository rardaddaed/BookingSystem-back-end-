using Autofac;
using System.Reflection;
using Module = Autofac.Module;

namespace BookingSystem.Repository
{
  public class RepositoryModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
        .Where(t => t.Name.EndsWith("Repository"))
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();

      base.Load(builder);
    }
  }
}
