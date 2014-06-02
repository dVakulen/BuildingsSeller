// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="nixsolutions">
//   (c) by nix
// </copyright>
// <summary>
//   The mvc application.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.UI.WebControls;
using BuildSeller.Infra;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using WebTester.Controllers;

namespace BuildSeller
{
    /// <summary>
    /// The mvc application.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// The application_ authenticate request.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>

        void Application_Error(Object sender, EventArgs e)
        {/*
            Exception exception = Server.GetLastError();

            HttpException httpException = exception as HttpException;

            var routeData = new RouteData();


            if (httpException != null)
            {
                string action;

                switch (httpException.GetHttpCode())
                {
                    case 401:
                        return;
                    case 404:
                        routeData.Values["Err"] = "Looks like page yore searching are not in this site";
                        break;
                    case 500:
                        routeData.Values["Err"] = "Server has hard time";
                        break;
                    default:
                        routeData.Values["Err"] = "Something bad had happened , were working on it";
                        break;
                }
            }
            else
            {
                routeData.Values["Err"] = "Something bad had happened , were working on it";
            }

            Response.Clear();
            Server.ClearError();
            routeData.Values["controller"] = "Home";
            routeData.Values["action"] = "ErrorPage";
            Response.StatusCode = 200;
            IController controller = new HomeController();
            var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
            controller.Execute(rc);*/
        }

        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            if (context.Response.Status.Substring(0, 3).Equals("401"))
            {

                Server.ClearError();
                Response.Clear();
                context.Response.ClearContent();

               IController controller = new HomeController();
                var routeData = new RouteData();

                routeData.Values["Err"] = "You do not have permission to view this page";
                routeData.Values["controller"] = "Home";
                Response.StatusCode = 500;
                routeData.Values["action"] = "ErrorPage";
                var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
                controller.Execute(rc);
            }
        }
        protected void Application_Start()
        {
            WindsorConfigurator.Configure();
            ConfigureWindsor(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configure(c => WebApiConfig.Register(c, container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Bootstrapper.Bootstrap(); 
        



        }
        public static void ConfigureWindsor(HttpConfiguration configuration)
        {
            container = new WindsorContainer();
            container.Install(FromAssembly.This());
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, true));
            var dependencyResolver = new WindsorDependencyResolver(container);
            configuration.DependencyResolver = dependencyResolver;
        }

        private static  IWindsorContainer container;
 
 

        /// <summary>
        /// The application_ post authenticate request.
        /// </summary>
        protected void Application_PostAuthenticateRequest()
        {
            HttpCookie authCookie = this.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var identity = new GenericIdentity(authTicket.Name, "Forms");
                var principal = new GenericPrincipal(identity, new string[] { });
                this.Context.User = principal;
            }
        }
        protected void Application_End()
        {
            container.Dispose();
            base.Dispose();
        }

    }
}