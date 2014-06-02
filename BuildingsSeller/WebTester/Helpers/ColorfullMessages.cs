
namespace BuildSeller.Helpers
{

    public static class ColorfullMessages
    {

        public static string SetDivs(string message, MessageType type)
        {
            return "<div class='alert alert-" + type + "'>" + message + "</div>";
        }
    }
}
