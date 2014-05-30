
namespace DAL
{
    using System.Data.Entity;

  
    public interface IDbContextFactory
    {
      
        DbContext GetContext();
    }
}