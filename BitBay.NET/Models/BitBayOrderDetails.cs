using System;
using System.Collections.Generic;
using System.Text;

namespace BitBay.NET.Models
{
    public class BitBayOrderDetails
    {
        public string Currency { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
    }
}
