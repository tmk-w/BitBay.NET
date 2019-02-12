using System;
using System.Collections.Generic;
using System.Text;

namespace BitBay.NET.Models
{
    public class BitBayAll : BitBayTicker
    {
        public IEnumerable<IEnumerable<double>> Asks { get; set; }
        public IEnumerable<IEnumerable<double>> Bids { get; set; }
        public IEnumerable<BitBayTrades> Transactions { get; set; }
    }
}
