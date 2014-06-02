
using System.Collections.Generic;

namespace BuildSeller.Core.Model
{

    public class Role : Entity
    {

        public virtual string Name { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
