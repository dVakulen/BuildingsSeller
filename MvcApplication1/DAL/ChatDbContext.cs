
namespace DAL
{
    using System.Data.Entity;

    using Core.Model;

    /// <summary>
    /// The build db context.
    /// </summary>
    public class ChatDbContext : DbContext
    {
      
        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        public DbSet<Users> Users { get; set; }

        public DbSet<Message> Messages { get; set; }


        /// <summary>
        /// The on model creating.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}