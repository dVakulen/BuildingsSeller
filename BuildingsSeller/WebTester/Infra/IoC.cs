
using System;
using Castle.Windsor;

namespace BuildSeller.Infra
{

    public static class IoC
    {

        private static readonly object LockObj = new object();

        private static IWindsorContainer container = new WindsorContainer();

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

        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        public static object Resolve(Type type)
        {
            return container.Resolve(type);
        }
    }
}
