using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BitBay.NET.Models
{
    public class BitBayOpenOrderDetails
    {
        [JsonProperty("order_id")]
        public string OrderId { get; set; }
        [JsonProperty("order_currency")]
        public string OrderCurrency { get; set; }
        [JsonProperty("order_date")]
        public string OrderDate { get; set; }
        [JsonProperty("payment_currency")]
        public string PaymentCurrency { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public double Units { get; set; }
        [JsonProperty("start_units")]
        public double StartUnits { get; set; }
        [JsonProperty("current_price")]
        public double CurrentPrice { get; set; }
        [JsonProperty("start_price")]
        public double StartPrice { get; set; }
    }
}
