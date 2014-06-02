
using System;

namespace BuildSeller.Models
{

    public class RealtySearchModel
    {

        public decimal PriceLow { get; set; }

        public decimal PriceHigh { get; set; }

        public float SquareLow { get; set; }

        public float SquareHigh { get; set; }

        public string Town { get; set; }

        public string Category { get; set; }

        public bool IsForRent { get; set; }

        public DateTime CreatedLow { get; set; }

        public DateTime CreatedHigh { get; set; }
    }
}
