using System;
using System.Collections.Generic;
using System.Text;

namespace BitBay.NET.Models
{
    public class BitBayTicker
    {
        public double Max { get; set; }
        public double Min { get; set; }
        public double Last { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
        public double Vwap { get; set; }
        public double Average { get; set; }
        public double Volume { get; set; }
    }
}
