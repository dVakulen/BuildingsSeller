
using System.Collections.Generic;

namespace BuildSeller.Core.Model
{

    public class BuildCategories : Entity
    {

        public string CatName { get; set; }

        public IList<Realty> Realties { get; set; }
    }
}
