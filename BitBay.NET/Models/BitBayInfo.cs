using System;
using System.Collections.Generic;
using System.Text;

namespace BitBay.NET.Models
{
    public class BitBayInfo
    {
        public string Currency { get; set; }
        public double Available { get; set; }
        public double Locked { get; set; }
    }
}
