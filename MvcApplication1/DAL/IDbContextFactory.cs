// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDbContextFactory.cs" company="nixsolutions">
//   (c) by nix
// </copyright>
// <summary>
//   The DbContextFactory interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DAL
{
    using System.Data.Entity;

    /// <summary>
    /// The DbContextFactory interface.
    /// </summary>
    public interface IDbContextFactory
    {
        /// <summary>
        /// The get context.
        /// </summary>
        /// <returns>
        /// The <see cref="DbContext"/>.
        /// </returns>
        DbContext GetContext();
    }
}