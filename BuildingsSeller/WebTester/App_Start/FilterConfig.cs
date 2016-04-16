// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterConfig.cs" company="nixsolutions">
//   (c) by nix
// </copyright>
// <summary>
//   The filter config.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;

namespace BuildSeller
{
    /// <summary>
    ///     The filter config.
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// The register global filters.
        /// </summary>
        /// <param name="filters">
        /// The filters.
        /// </param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           // filters.Add(new HandleErrorAttribute());
        }
    }
}
