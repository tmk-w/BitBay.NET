﻿using System;
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
        private readonly string _historyEndpoint = "history";
        private readonly string _withdrawEndpoint = "withdraw";
        private readonly string _transferEndpoint = "transfer";

        private readonly HttpClient _httpClient;

        private double _timeOffset;

        public BitBayClient(ClientConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_publicBaseAddress);
            _apiKey = configuration.ApiKey;
            _apiSecret = configuration.ApiSecret;
            _timeOffset = configuration.TimeOffset;
        }

        #region Private methods

        public BitBayTransfer Transfer(string currency, double quantity, string address) =>
            TransferAsync(currency, quantity, address).Result;

        public async Task<BitBayTransfer> TransferAsync(string currency, double quantity, string address)
        {
            var content = new Dictionary<string, string>()
            {
                { "currency", currency },
                { "quantity", quantity.ToString(CultureInfo.InvariantCulture) },
                { "address", address }
            };

            return await ExecutePostAsync<BitBayTransfer>(_transferEndpoint, content);
        }

        public BitBayWithdraw Withdraw(string currency, double quantity, string accountNumber, bool express,
            string bicNumber) => WithdrawAsync(currency, quantity, accountNumber, express, bicNumber).Result;

        public async Task<BitBayWithdraw> WithdrawAsync(string currency, double quantity, string accountNumber,
            bool express, string bicNumber)
        {
            var content = new Dictionary<string, string>()
            {
                { "currency", currency },
                { "quantity", quantity.ToString(CultureInfo.InvariantCulture) },
                { "accountNumber", accountNumber },
                { "express", express.ToString() },
                { "bic", bicNumber }
            };

            return await ExecutePostAsync<BitBayWithdraw>(_withdrawEndpoint, content);
        }

        public IEnumerable<BitBayHistory> GetHistory(string currency, int limit) =>
            GetHistoryAsync(currency, limit).Result;

        public async Task<IEnumerable<BitBayHistory>> GetHistoryAsync(string currency, int limit)
        {
            var content = new Dictionary<string, string>()
            {
                { "currency", currency },
                { "limit", limit.ToString() }
            };

            return await ExecutePostAsync<IEnumerable<BitBayHistory>>(_historyEndpoint, content);
        }

        public IEnumerable<BitBayTransactionDetails> GetTransactions() => GetTransactionsAsync().Result;

        public async Task<IEnumerable<BitBayTransactionDetails>> GetTransactionsAsync()
        {
            return await ExecutePostAsync<IEnumerable<BitBayTransactionDetails>>(_transactionsEndpoint);
        }

        public IEnumerable<BitBayTransactionDetails> GetTransactions(string market) => GetTransactionsAsync(market).Result;

        public async Task<IEnumerable<BitBayTransactionDetails>> GetTransactionsAsync(string market)
        {
            var content = new Dictionary<string, string>()
            {
                { "market", market }
            };

            return await ExecutePostAsync<IEnumerable<BitBayTransactionDetails>>(_transactionsEndpoint, content);
        }

        public IEnumerable<BitBayOpenOrderDetails> GetOpenOrders() => GetOpenOrdersAsync().Result;

        public async Task<IEnumerable<BitBayOpenOrderDetails>> GetOpenOrdersAsync()
        {
            return await ExecutePostAsync<IEnumerable<BitBayOpenOrderDetails>>(_openOrdersEndpoint);
        }

        public IEnumerable<BitBayOpenOrderDetails> GetOpenOrders(double limit) => GetOpenOrdersAsync(limit).Result;

        public async Task<IEnumerable<BitBayOpenOrderDetails>> GetOpenOrdersAsync(double limit)
        {
            var content = new Dictionary<string, string>()
            {
                {"limit", limit.ToString()}
            };

            return await ExecutePostAsync<IEnumerable<BitBayOpenOrderDetails>>(_openOrdersEndpoint, content);
        }

        public BitBayInfo GetInfo() => GetInfoAsync().Result;

        public async Task<BitBayInfo> GetInfoAsync()
        {
            return await ExecutePostAsync<BitBayInfo>(_infoEndpoint);
        }

        public BitBayInfo GetInfo(string currency) => GetInfoAsync(currency).Result;

        public async Task<BitBayInfo> GetInfoAsync(string currency)
        {
            var content = new Dictionary<string, string>()
            {
                { "currency", currency }
            };

            return await ExecutePostAsync<BitBayInfo>(_infoEndpoint, content);
        }

        public BitBayTrade Trade(string type, string currency, double amount, string paymentCurrency, double rate) =>
            TradeAsync(type, currency, amount, paymentCurrency, rate).Result;

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

        public BitBayCancel Cancel(string orderId) => CancelAsync(orderId).Result;

        public async Task<BitBayCancel> CancelAsync(string orderId)
        {
            var content = new Dictionary<string, string>()
            {
                { "id", orderId },
            };

            return await ExecutePostAsync<BitBayCancel>(_cancelEndpoint, content);
        }

        public BitBayMarketOrders GetMarketOrders(string currency, string paymentCurrency) =>
            GetMarketOrdersAsync(currency, paymentCurrency).Result;

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

        public BitBayAll GetAll(string market) => GetAllAsync(market).Result;

        public async Task<BitBayAll> GetAllAsync(string market)
        {
            var address = $"{market}/{_allEndpoint}";

            return await ExecuteGetRequest<BitBayAll>(address);
        }

        public BitBayMarket GetMarket(string market) => GetMarketAsync(market).Result;

        public async Task<BitBayMarket> GetMarketAsync(string market)
        {
            var address = $"{market}/{_marketEndpoint}";

            return await ExecuteGetRequest<BitBayMarket>(address);
        }

        public IEnumerable<BitBayTrades> GetTrades(string market) => GetTradesAsync(market).Result;

        public async Task<IEnumerable<BitBayTrades>> GetTradesAsync(string market)
        {
            var address = $"{market}/{_tradesEndpoint}";

            return await ExecuteGetRequest<IEnumerable<BitBayTrades>>(address);
        }

        public BitBayOrderBook GetOrderBook(string market) => GetOrderBookAsync(market).Result;

        public async Task<BitBayOrderBook> GetOrderBookAsync(string market)
        {
            var address = $"{market}/{_orderBookEndpoint}";

            return await ExecuteGetRequest<BitBayOrderBook>(address);
        }

        public BitBayTicker GetTicker(string market) => GetTickerAsync(market).Result;

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
            var moment = GetTimestamp();

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

        private long ToUnixTimestamp(DateTime time)
        {
            return (long)(time - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        private string GetTimestamp()
        {
            return ToUnixTimestamp(DateTime.UtcNow.AddSeconds(_timeOffset)).ToString();
        }
    }
}
