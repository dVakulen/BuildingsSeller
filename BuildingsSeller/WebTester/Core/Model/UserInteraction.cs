
namespace BuildSeller.Core.Model
{

    public class UserInteraction : Entity
    {

        public string Name { get; set; }

        public bool IsLiked { get; set; }

        public bool IsInteracted { get; set; }
    }
}
