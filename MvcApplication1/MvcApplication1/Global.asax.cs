using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BuildSeller.Infra;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
namespace MvcApplication1
{
    using System.Web.Http.Dispatcher;

    using BuildSeller;
    using BuildSeller.Service;

    using Castle.MicroKernel.Resolvers.SpecializedResolvers;
    using Castle.Windsor;
    using Castle.Windsor.Installer;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            WindsorReg.Initialize(); 

        
            ConfigureWindsor(GlobalConfiguration.Configuration);
        
           //GlobalConfiguration.Configure(c => WebApiConfig.Register(c, container));
           
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        public static void ConfigureWindsor(HttpConfiguration configuration)
        {
            container = IoC.Container;//new WindsorContainer();
            container.Install(FromAssembly.This());
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, true));
            var dependencyResolver = new WindsorDependencyResolver(container);
            configuration.DependencyResolver = dependencyResolver;


            GlobalConfiguration.Configuration.Services.Replace(
     typeof(IHttpControllerActivator),
     new WindsorCompositionRoot(container));


            // ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory11(IoC.Container));
        }

      protected void Application_End()
        {
            container.Dispose();
            base.Dispose();
        }

        private static IWindsorContainer container;
    }
}