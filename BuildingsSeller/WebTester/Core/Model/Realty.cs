
using System;
using System.Collections.Generic;

namespace BuildSeller.Core.Model
{
    using System.Reflection;

    using Newtonsoft.Json;

    [JsonObject]
    public class Realty : Entity
    {
        
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
