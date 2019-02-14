using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BitBay.NET.Models
{
    public class BitBayHistory
    {
        public double Amount { get; set; }
        [JsonProperty("balance_after")]
        public double BalanceAfter { get; set; }

        public string Currency { get; set; }
        public DateTime Time { get; set; }
        public string Comment { get; set; }
    }
}
