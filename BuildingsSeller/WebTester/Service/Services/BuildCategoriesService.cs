
using System.Linq;
using BuildSeller.Core.Model;
using BuildSeller.Core.Repository;

namespace BuildSeller.Service
{

    public class BuildCategoriesService : CrudService<BuildCategories>, IBuildCategoriesService
    {

        public BuildCategoriesService(IRepo<BuildCategories> repo)
            : base(repo)
        {
        }

        public bool IsUnique(BuildCategories cat)
        {
            return !this.Repo.Where(o => o.CatName == cat.CatName).Any();
        }
    }
}
