// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CastleWindsorControllerFactory.cs" company="nixsolutions">
//   (c) by nix
// </copyright>
// <summary>
//   The windsor controller factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
namespace BuildSeller
{
    /// <summary>
    ///     The windsor controller factory.
    /// </summary>
    /// 
    public class WindsorCompositionRoot : IHttpControllerActivator
    {
        private readonly IWindsorContainer container;

        public WindsorCompositionRoot(IWindsorContainer container)
        {
            this.container = container;
        }

        public IHttpController Create(
            HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            var controller =
                (IHttpController)container.Resolve(controllerType);

            request.RegisterForDispose(
                new Release(
                    () => container.Release(controller)));

            return controller;
        }

        private class Release : IDisposable
        {
            private readonly Action _release;

            public Release(Action release)
            {
                _release = release;
            }

            public void Dispose()
            {
                _release();
            }
        }
    }
    /* public class WindsorCompositionRoot : IHttpControllerActivator
     {
         private readonly IWindsorContainer container;

         public WindsorCompositionRoot(IWindsorContainer container)
         {
             this.container = container;
         }

         public IHttpController Create(
             HttpRequestMessage request,
             HttpControllerDescriptor controllerDescriptor,
             Type controllerType)
         {
             var controller =
                 (IHttpController)this.container.Resolve(controllerType);

             request.RegisterForDispose(
                 new Release(
                     () => this.container.Release(controller)));

             return controller;
         }

         private class Release : IDisposable
         {
             private readonly Action release;

             public Release(Action release)
             {
                 this.release = release;
             }

             public void Dispose()
             {
                 this.release();
             }
         }
     }*/
     public class WindsorControllerFactory11 : DefaultControllerFactory
     {
         /// <summary>
         ///     The container.
         /// </summary>
         private readonly IWindsorContainer container;

         /// <summary>
         /// Initializes a new instance of the <see cref="WindsorControllerFactory"/> class.
         /// </summary>
         /// <param name="container">
         /// The container.
         /// </param>
         public WindsorControllerFactory11(IWindsorContainer container)
         {
             this.container = container;
             IEnumerable<Type> controllerTypes =
                 from t in Assembly.GetExecutingAssembly().GetTypes()
                 where typeof(IController).IsAssignableFrom(t)
                 select t;
             foreach (Type t in controllerTypes)
             {
                 container.Register(Component.For(t).LifeStyle.Transient);
             }

           //  container.Register(Component.For(typeof( BuildApiController)).LifeStyle.Transient);
         }

         /// <summary>
         /// The get controller instance.
         /// </summary>
         /// <param name="requestContext">
         /// The request context.
         /// </param>
         /// <param name="controllerType">
         /// The controller type.
         /// </param>
         /// <returns>
         /// The <see cref="IController"/>.
         /// </returns>
         protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
         {
             if (controllerType == null)
             {
                 return null;
             }

             return (IController) this.container.Resolve(controllerType);
         }
       
     }
}