#region Using statements
using System.Data.Linq;
using System.Diagnostics.CodeAnalysis;
#endregion

namespace TOS.WinPhone.DataLayer.Context
{
    public class BuildContext : DataContext
    {
        private const string ConnectionString = "DataSource=isostore:/Test.sdf";

        #region Constructor
        public BuildContext()
            : this(ConnectionString)
        {
        }

        public BuildContext(string connectionString)
            : base(connectionString)
        {
            if (!this.DatabaseExists())
            {
                this.CreateDatabase();
             
            }

        }
        
        #endregion

        #region Tables

        #endregion
    }
}