
namespace DAL
{
    using System.Data.Entity;


    public class DbContextFactory : IDbContextFactory
    {

        private readonly ChatDbContext dbContext;


        public DbContextFactory()
        {
            this.dbContext = new ChatDbContext();
        }


        public DbContext GetContext()
        {
            return this.dbContext;
        }
    }
}