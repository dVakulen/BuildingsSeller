// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DBContextFactory.cs" company="nixsolutions">
//   (c) by nix
// </copyright>
// <summary>
//   The db context factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DAL
{
    using System.Data.Entity;

    /// <summary>
    /// The db context factory.
    /// </summary>
    public class DbContextFactory : IDbContextFactory
    {
        /// <summary>
        /// The db context.
        /// </summary>
        private readonly ChatDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextFactory"/> class.
        /// </summary>
        public DbContextFactory()
        {
            this.dbContext = new ChatDbContext();
        }

        /// <summary>
        /// The get context.
        /// </summary>
        /// <returns>
        /// The <see cref="DbContext"/>.
        /// </returns>
        public DbContext GetContext()
        {
            return this.dbContext;
        }
    }
}