
using System;
using System.Collections.Generic;

namespace BuildSeller.Core.Model
{
    using System.Reflection;

    public class Realty : Entity
    {

        public string Named { get; set; }

        public byte[] Picture { get; set; }

        public decimal Price { get; set; }

        public float Square { get; set; }

        public double MapPosX { get; set; }

        public double MapPosY { get; set; } 
        public string Address { get; set; }

        public string Description { get; set; }

        public bool IsForRent { get; set; }

        public bool IsSold { get; set; }

        public DateTime Created { get; set; }

        public BuildCategories BuildCategory { get; set; }

        public Users Owner { get; set; }

        public List<ImageAttachments> Photos { get; set; }
    }
}
