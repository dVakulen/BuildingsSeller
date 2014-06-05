namespace Build.DataLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using BuildSeller.Core.Model;

    using Newtonsoft.Json;

    [JsonObject]
    public class Realty : Entity
    {

        public BitmapImage PictureSource

        {

            set
            {
                if (value != null )
                {
                 
                    using (MemoryStream ms = new MemoryStream())
                    {
                        WriteableBitmap btmMap = new WriteableBitmap
                            (value);
                       
                        Extensions.SaveJpeg(btmMap, ms,
                            value.PixelWidth, value.PixelHeight, 0, 100);

                        Picture= ms.ToArray();
                    }
                }
            }
            get
            {
              
                BitmapImage biImg = new BitmapImage();
                if (Picture == null)
                {
                    return null;
                }
                MemoryStream ms = new MemoryStream(Picture);
               
                biImg.SetSource(ms);
                return biImg;
            }
        }

        [JsonProperty]
        public string Named { get; set; }
        [JsonProperty]
        public byte[] Picture { get; set; }
        [JsonProperty]
        public decimal Price { get; set; }
        [JsonProperty]
        public float Square { get; set; }
        [JsonProperty]
        public double MapPosX { get; set; }
        [JsonProperty]
        public double MapPosY { get; set; }
        [JsonProperty]
        public string Address { get; set; }
        [JsonProperty]
        public string Description { get; set; }
        [JsonProperty]
        public bool IsForRent { get; set; }
        [JsonProperty]
        public bool IsSold { get; set; }
        [JsonProperty]
        public DateTime Created { get; set; }
        [JsonProperty]
        public BuildCategories BuildCategory { get; set; }
        [JsonProperty]
        public Users Owner { get; set; }
        [JsonProperty]
        public List<ImageAttachments> Photos { get; set; }
    }
}
