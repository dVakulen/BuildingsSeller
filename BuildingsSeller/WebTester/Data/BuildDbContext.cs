
using System.Data.Entity;
using BuildSeller.Core.Model;

namespace BuildSeller.Data
{

    public class BuildDbContext : DbContext
    {

        public DbSet<Subscribe> Subscribers { get; set; }

        public DbSet<Users> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<ImageAttachments> ImageAttachment { get; set; }

        public DbSet<BuildCategories> BuildCategorieses { get; set; }

        public DbSet<UserInteraction> UserInteractions { get; set; }

        public DbSet<Realty> Realties { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
