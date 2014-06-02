// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISessionfact.cs" company="nixsolutions">
//   (c) by nix
// </copyright>
// <summary>
//   The Sessionfact interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using NHibernate;

namespace WebTester.Data
{
    public interface ISessionfact
    {
        /// <summary>
        ///     The get current session.
        /// </summary>
        /// <returns>
        ///     The <see cref="ISession" />.
        /// </returns>
        ISession GetCurrentSession();

        /// <summary>
        ///     The get session.
        /// </summary>
        /// <returns>
        ///     The <see cref="ISession" />.
        /// </returns>
        ISession GetSession();
    }
}