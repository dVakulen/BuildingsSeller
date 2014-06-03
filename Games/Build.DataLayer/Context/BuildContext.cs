#region Using statements



#endregion

namespace Build.DataLayer.Context
{
    using System.Data.Linq;

    using Build.DataLayer.Model;

    public class BuildContext : DataContext
    {
        private const string ConnectionString = "DataSource=isostore:/Build.sdf";

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

        public Table<CurrentUser> CurrentUser;
        #endregion
    }
}