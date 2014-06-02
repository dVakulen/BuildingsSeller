
using System.Data.Entity;

namespace BuildSeller.Data
{

    public interface IDbContextFactory
    {

        DbContext GetContext();
    }
}
