using System;
using System.Collections.Generic;
using System.Text;

namespace BitBay.NET.Models
{
    public class BitBayInfo
    {
        public bool Success { get; set; }
        public double Fee { get; set; }
        public BitBayBalancesInfo Balances { get; set; }
        public BitBayAddressInfo Addresses { get; set; }
    }
}
