
namespace BuildSeller.Core.Model
{

    public class Subscribe : Entity
    {

        public string Name { get; set; }

        public Users Subscriber { get; set; }

        public string Description { get; set; }
    }
}
