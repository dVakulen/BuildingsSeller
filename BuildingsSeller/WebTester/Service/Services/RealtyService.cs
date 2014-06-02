
using BuildSeller.Core.Model;
using BuildSeller.Core.Repository;

namespace BuildSeller.Service
{

    public class RealtyService : CrudService<Realty>, IRealtyService
    {

        public RealtyService(IRepo<Realty> repo)
            : base(repo)
        {
        }
    }
}
