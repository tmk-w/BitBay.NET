using System;
using System.Collections.Generic;
using System.Text;

namespace BitBay.NET.Models
{
    public class BitBayTransactionDetails
    {
        public double Amount { get; set; }
        public double Price { get; set; }
        public double Rate { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Market { get; set; }
    }
}
