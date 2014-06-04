#region Using Directives



#endregion
namespace Build.DataLayer.Model
{
    using System.Windows.Media.Imaging;

    public class Photo : ImageCard
    {
        #region Public Properties

        public BitmapImage Image { get; set; }

        #endregion
    }
}