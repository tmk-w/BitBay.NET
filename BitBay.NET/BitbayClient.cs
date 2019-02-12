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
        private readonly string _tradesEndpoint = "trades";
        private readonly string _marketEndpoint = "market";
        private readonly string _allEndpoint = "all";

        private readonly HttpClient _httpClient;

        public BitBayClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_publicBaseAddress);
        }

        public async Task<BitBayAll> GetAllAsync(string market)
        {
            var address = $"{market}/{_allEndpoint}";

            return await ExecuteGetRequest<BitBayAll>(address);
        }

        public async Task<BitBayMarket> GetMarketAsync(string market)
        {
            var address = $"{market}/{_marketEndpoint}";

            return await ExecuteGetRequest<BitBayMarket>(address);
        }

        public async Task<IEnumerable<BitBayTrades>> GetTradesAsync(string market)
        {
            var address = $"{market}/{_tradesEndpoint}";

            return await ExecuteGetRequest<IEnumerable<BitBayTrades>>(address);
        }

        public async Task<BitBayOrderBook> GetOrderBookAsync(string market)
        {
            var address = $"{market}/{_orderBookEndpoint}";

            return await ExecuteGetRequest<BitBayOrderBook>(address);
        }

        public async Task<BitBayTicker> GetTickerAsync(string market)
        {
            var address = $"{market}/{_tickerEndpoint}";

            return await ExecuteGetRequest<BitBayTicker>(address);
        }

        private async Task<T> ExecuteGetRequest<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync($"{endpoint}.json");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var responseData = JsonConvert.DeserializeObject<T>(json);

            return responseData;
        }
    }
}
