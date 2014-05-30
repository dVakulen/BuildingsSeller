// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bootstrapper.cs" company="nixsolutions">
//   (c) by nix
// </copyright>
// <summary>
//   The bootstrapper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;

namespace BuildSeller
{
    using BuildSeller.Infra;

    /// <summary>
    /// The bootstrapper.
    /// </summary>
    public class Bootstrapper
    {
        /// <summary>
        ///     The bootstrap.
        /// </summary>
        public static void Bootstrap()
        {
            //    GlobalConfiguration.Configuration.Services.Replace(
           //      typeof(IHttpControllerActivator),
            //     new WindsorControllerFactory11(IoC.Container));
           ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory11(IoC.Container));
        }
    }
}