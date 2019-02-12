using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BitBay.NET.Models;
using Newtonsoft.Json;

namespace BitBay.NET
{
    public class BitBayClient
    {
        private readonly string _publicBaseAddress = "https://bitbay.net/API/Public/";

        private readonly string _orderBookEndpoint = "orderbook";
        private readonly string _tickerEndpoint = "ticker";

        private readonly HttpClient _httpClient;

        public BitBayClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_publicBaseAddress);
        }

        public async Task<BitBayOrderBook> GetOrderBookAsync(string market)
        {
            var address = $"{market}/{_orderBookEndpoint}.json";

            var response = await _httpClient.GetAsync(address);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var responseData = JsonConvert.DeserializeObject<BitBayOrderBook>(json);

            return responseData;
        }

        public async Task<BitBayTicker> GetTickerAsync(string market)
        {
            var address = $"{market}/{_tickerEndpoint}.json";

            var response = await _httpClient.GetAsync(address);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var responseData = JsonConvert.DeserializeObject<BitBayTicker>(json);

            return responseData;
        }

    }
}
