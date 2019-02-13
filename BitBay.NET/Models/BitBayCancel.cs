using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BitBay.NET.Models
{
    public class BitBayCancel
    {
        public bool Success { get; set; }
        [JsonProperty("order_id")]
        public string OrderId { get; set; }
    }
}
