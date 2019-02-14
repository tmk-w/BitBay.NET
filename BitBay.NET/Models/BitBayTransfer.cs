using Newtonsoft.Json;

namespace BitBay.NET.Models
{
    public class BitBayTransfer
    {
        public bool Status { get; set; }
        public string Error { get; set; }
        [JsonProperty("insert_id")]
        public string WithdrawalId { get; set; }
    }
}
