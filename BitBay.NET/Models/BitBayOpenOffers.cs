using System;
using System.Collections.Generic;
using System.Text;

namespace BitBay.NET.Models
{
    public class BitBayOpenOffers
    {
        public IEnumerable<BitBayOrderDetails> Asks { get; set; }
        public IEnumerable<BitBayOrderDetails> Bids { get; set; }
    }
}
