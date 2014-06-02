
using System.Data.Entity;

namespace BuildSeller.Data
{

    public class DbContextFactory : IDbContextFactory
    {

        private readonly BuildDbContext dbContext;

        public DbContextFactory()
        {
            this.dbContext = new BuildDbContext();
        }

        public DbContext GetContext()
        {
            return this.dbContext;
        }
    }
}
