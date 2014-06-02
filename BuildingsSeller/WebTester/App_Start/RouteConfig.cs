// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="nixsolutions">
//   (c) by nix
// </copyright>
// <summary>
//   The route config.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;
using System.Web.Routing;

namespace BuildSeller
{
    /// <summary>
    ///     The route config.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// The register routes.
        /// </summary>
        /// <param name="routes">
        /// The routes.
        /// </param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "login", 
                "login", 
                new { controller = "Account", action = "Login" });
            routes.MapRoute(
                "logoff", 
                "logoff", 
                new { controller = "Account", action = "LogOff" });
            routes.MapRoute("Empty", string.Empty, new { controller = "Home", action = "Index" });

            routes.MapRoute(
                "MyCabinet", 
                "MyCabinet", 
                new { controller = "Account", action = "Manage" });
            routes.MapRoute(
                "RolesList", 
                "UserManager/GetRoles", 
                new { controller = "UserManager", action = "GetRoles", userName = UrlParameter.Optional });
            routes.MapRoute(
                "Find", 
                "find/{sity}/{category}", 
                new
                {
                    controller = "Realty", 
                    action = "Find", 
                    sity = UrlParameter.Optional, 
                    category = UrlParameter.Optional
                });


            routes.MapRoute("Default", 
                "{controller}/{action}/{id}", 
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}