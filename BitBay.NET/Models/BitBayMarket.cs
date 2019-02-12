using System;
using System.Collections.Generic;
using System.Text;

namespace BitBay.NET.Models
{
    public class BitBayMarket : BitBayOrderBook
    {
        public IEnumerable<BitBayTrades> Transactions { get; set; }
    }
}
