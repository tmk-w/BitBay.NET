using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace BitBay.NET.Models
{
    public class BitBayTrade
    {
        public bool Success { get; set; }
        public double Amount { get; set; }
        public double Rate { get; set; }
        public double Price { get; set; }
        public double Fee { get; set; }
        [JsonProperty("fee_currency")]
        public string FeeCurrency { get; set; }
        [JsonProperty("order_id")]
        public string OrderId { get; set; }
        public string Error { get; set; }
    }
}
