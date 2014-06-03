using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragDropPhoneApp.Context
{
    using System.Data.Linq;

    using DragDropPhoneApp.Model;

    public class GameContext : DataContext
    {
        private const string ConnectionString = "DataSource=isostore:/Test.sdf";
        public Table<RankTableEntry> RankTableEntries;
        public GameContext()
            : this(ConnectionString)
        {
        }

        public GameContext(string connectionString)
            : base(connectionString)
        {

            if (!this.DatabaseExists())
            {
                this.CreateDatabase();
            }

        }

    }
}
