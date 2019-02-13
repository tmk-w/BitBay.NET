using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using BitBay.NET.Models;
using Newtonsoft.Json;

namespace BitBay.NET
{
    public class BitBayClient
    {
        private readonly string _publicBaseAddress = "https://bitbay.net/API/Public/";
        private readonly string _privateBaseAddress = "https://bitbay.net/API/Trading/tradingApi.php";

        private readonly string _apiKey;
        private readonly string _apiSecret;

        private readonly string _orderBookEndpoint = "orderbook";
        private readonly string _tickerEndpoint = "ticker";
        private readonly string _tradesEndpoint = "trades";
        private readonly string _marketEndpoint = "market";
        private readonly string _allEndpoint = "all";

        private readonly string _infoEndpoint = "info";
        private readonly string _tradeEndpoint = "trade";
        private readonly string _cancelEndpoint = "cancel";
        private readonly string _marketOrdersEndpoint = "orderbook";
        private readonly string _openOrdersEndpoint = "orders";
        private readonly string _transactionsEndpoint = "transactions";

        private readonly HttpClient _httpClient;

        public BitBayClient(ClientConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_publicBaseAddress);
            _apiKey = configuration.ApiKey = configuration.ApiKey;
            _apiSecret = configuration.ApiSecret;
        }

        #region Private methods

        public async Task<IEnumerable<BitBayTransactionDetails>> GetTransactionsAsync()
        {
            return await ExecutePostAsync<IEnumerable<BitBayTransactionDetails>>(_transactionsEndpoint);
        }

        public async Task<IEnumerable<BitBayOpenOrderDetails>> GetOpenOrdersAsync()
        {
            return await ExecutePostAsync<IEnumerable<BitBayOpenOrderDetails>>(_openOrdersEndpoint);
        }

        public async Task<BitBayInfo> GetInfoAsync()
        {
            return await ExecutePostAsync<BitBayInfo>(_infoEndpoint);
        }

        public async Task<BitBayTrade> TradeAsync(string type, string currency, double amount, string paymentCurrency, double rate)
        {
            var content = new Dictionary<string, string>()
            {
                { "currency", currency },
                { "payment_currency", paymentCurrency },
                { "type", type },
                { "amount", amount.ToString(CultureInfo.InvariantCulture) },
                { "rate", rate.ToString(CultureInfo.InvariantCulture) },
            };

            return await ExecutePostAsync<BitBayTrade>(_tradeEndpoint, content);
        }

        public async Task<BitBayCancel> CancelAsync(string orderId)
        {
            var content = new Dictionary<string, string>()
            {
                { "id", orderId },
            };

            return await ExecutePostAsync<BitBayCancel>(_cancelEndpoint, content);
        }

        public async Task<BitBayMarketOrders> GetMarketOrdersAsync(string currency, string paymentCurrency)
        {
            var content = new Dictionary<string, string>()
            {
                { "order_currency", currency },
                { "payment_currency", paymentCurrency },
            };

            return await ExecutePostAsync<BitBayMarketOrders>(_marketOrdersEndpoint, content);
        }

        #endregion

        #region Public methods

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

        #endregion

        private async Task<T> ExecuteGetRequest<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync($"{endpoint}.json");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var responseData = JsonConvert.DeserializeObject<T>(json);

            return responseData;
        }

        private async Task<T> ExecutePostAsync<T>(string method, IDictionary<string, string> parameters = null)
        {
            var moment = await GetTonce();

            var content = new Dictionary<string, string>()
            {
                {"method", method},
                {"moment", moment.ToString()}
            };

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    content.Add(parameter.Key, parameter.Value);
                }
            }

            var queryParams = content
                .ToNameValueCollection()
                .ToQueryString();

            var signature = GetSignature(queryParams, _apiSecret);

            if (!_httpClient.DefaultRequestHeaders.Contains("API-Key"))
            {
                _httpClient.DefaultRequestHeaders.Add("API-Key", _apiKey);
            }

            _httpClient.DefaultRequestHeaders.Remove("API-Hash");

            _httpClient.DefaultRequestHeaders.Add("API-Hash", signature);

            var response = await _httpClient.PostAsync(_privateBaseAddress,
                new FormUrlEncodedContent(content));
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var responseData = JsonConvert.DeserializeObject<T>(json);

            return responseData;
        }

        private string GetSignature(string message, string _apiSecret)
        {
            var keyByte = Encoding.UTF8.GetBytes(_apiSecret);
            using (var hmacsha512 = new HMACSHA512(keyByte))
            {
                hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(message));
                return ByteToString(hmacsha512.Hash);
            }
        }

        private string ByteToString(byte[] buff)
        {
            string sbinary = "";
            for (int i = 0; i < buff.Length; i++)
                sbinary += buff[i].ToString("x2");
            return sbinary;
        }

        private async Task<int> GetTonce()
        {
            var response = await _httpClient.GetAsync("https://google.com", HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var date = response.Headers.Date;

            if (date == null)
            {
                date = DateTime.UtcNow;
            }

            return ((Int32)date.Value.UtcDateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
        }
    }
}
