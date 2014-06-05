using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Build.DataLayer.Model
{
    using System.Data.Linq.Mapping;

    [Table]
   public class CurrentUser
    {
        private Guid id;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public Guid Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }
        [Column]
        public string Login;

        [Column]
        public DateTime LoginTime;
        [Column]
        public string Password;
    }
}
