// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="nixsolutions">
//   (c) by nix
// </copyright>
// <summary>
//   The io c.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



using System;
using Castle.Windsor;

namespace BuildSeller.Infra
{
    /// <summary>
    /// The io c.
    /// </summary>
    public static class IoC
    {
        /// <summary>
        /// The lock obj.
        /// </summary>
        private static readonly object LockObj = new object();

        /// <summary>
        /// The container.
        /// </summary>
        private static IWindsorContainer container = new WindsorContainer();

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        public static IWindsorContainer Container
        {
            get { return container; }

            set
            {
                lock (LockObj)
                {
                    container = value;
                }
            }
        }

        /// <summary>
        /// The resolve.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        /// <summary>
        /// The resolve.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object Resolve(Type type)
        {
            return container.Resolve(type);
        }
    }
}