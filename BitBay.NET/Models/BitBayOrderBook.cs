using System;
using System.Collections.Generic;
using System.Text;

namespace BitBay.NET.Models
{
    public class BitBayOrderBook
    {
        public IEnumerable<IEnumerable<double>> Asks { get; set; }
        public IEnumerable<IEnumerable<double>> Bids { get; set; }
    }
}
