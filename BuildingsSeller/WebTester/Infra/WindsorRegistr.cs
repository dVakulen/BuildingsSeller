
using System;
using Castle.MicroKernel.Registration;

namespace BuildSeller.Infra
{

    public class WindsorRegistr
    {

        public static void RegisterSingleton(Type interfaceType, Type implementationType)
        {
            IoC.Container.Register(Component.For(interfaceType).ImplementedBy(implementationType).LifeStyle.Singleton);
        }

        public static void Register(Type interfaceType, Type implementationType)
        {
            IoC.Container.Register(
            Component.For(interfaceType).ImplementedBy(implementationType).LifeStyle.PerWebRequest);
        }

        public static void RegisterAllFromAssemblies(string a)
        {
            IoC.Container.Register(
            AllTypes.FromAssemblyNamed(a)
            .Pick()
            .WithService.FirstInterface()
            .Configure(o => o.LifestylePerWebRequest()));
        }
    }
}
