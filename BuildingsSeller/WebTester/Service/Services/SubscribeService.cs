
using BuildSeller.Core.Model;
using BuildSeller.Core.Repository;

namespace BuildSeller.Service
{

    public class SubscribeService : CrudService<Subscribe>, ISubscribeService
    {

        public SubscribeService(IRepo<Subscribe> repo)
            : base(repo)
        {
        }
    }
}
