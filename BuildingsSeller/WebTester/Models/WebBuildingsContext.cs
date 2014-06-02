
using System.Data.Entity;
using BuildSeller.Core.Model;

namespace BuildSeller.Models
{

    public class WebBuildingsContext : DbContext
    {

        public WebBuildingsContext()
            : base("name=WebBuildingsContext")
        {
        }

        public DbSet<Users> Users { get; set; }

        public DbSet<BuildCategories> BuildCategories { get; set; }

        public DbSet<Realty> Realties { get; set; }

        public DbSet<Subscribe> Subscribes { get; set; }
    }
}
