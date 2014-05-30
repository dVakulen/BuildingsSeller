// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindsorRegistr.cs" company="nixsolutions">
//   (c) by nix
// </copyright>
// <summary>
//   The windsor registr.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



using System;
using Castle.MicroKernel.Registration;

namespace BuildSeller.Infra
{
    /// <summary>
    /// The windsor registr.
    /// </summary>
    public class WindsorRegistr
    {
        /// <summary>
        /// The register singleton.
        /// </summary>
        /// <param name="interfaceType">
        /// The interface type.
        /// </param>
        /// <param name="implementationType">
        /// The implementation type.
        /// </param>
        public static void RegisterSingleton(Type interfaceType, Type implementationType)
        {
            IoC.Container.Register(Component.For(interfaceType).ImplementedBy(implementationType).LifeStyle.Singleton);
        }

        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="interfaceType">
        /// The interface type.
        /// </param>
        /// <param name="implementationType">
        /// The implementation type.
        /// </param>
        public static void Register(Type interfaceType, Type implementationType)
        {
                IoC.Container.Register(
              Component.For(interfaceType).ImplementedBy(implementationType).LifeStyle.PerWebRequest);
           
          
        }


        /// <summary>
        /// The register all from assemblies.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
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