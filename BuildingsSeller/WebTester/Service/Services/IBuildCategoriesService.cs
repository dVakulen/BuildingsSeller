
using BuildSeller.Core.Model;
using BuildSeller.Core.Service;

namespace BuildSeller.Service
{

    public interface IBuildCategoriesService : ICrudService<BuildCategories>
    {

        bool IsUnique(BuildCategories cat);
    }
}
