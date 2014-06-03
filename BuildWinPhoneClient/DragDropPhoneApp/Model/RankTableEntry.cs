using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragDropPhoneApp.Model
{
    using System.Data.Linq.Mapping;

  
        [Table]
        public class RankTableEntry
        {
            #region Fields

            private Guid id;

            #endregion

            #region Public Properties

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
            public string UserName { get; set; }
            [Column]
            public string TimePassed { get; set; }
            #endregion
        }
   
}
