using System;
using System.Collections.Generic;
using System.Text;

namespace BitBay.NET.Models
{
    public class BitBayTrades
    {
        public string Date { get; set; }
        public double Price { get; set; }
        public double Amount { get; set; }
        public string TId { get; set; }
    }
}
