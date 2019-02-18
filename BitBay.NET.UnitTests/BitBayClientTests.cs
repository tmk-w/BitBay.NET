using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BitBay.NET.UnitTests
{

    [TestClass]
    public class BitBayClientTests
    {
        private BitBayClient client;

        [TestInitialize]
        public void Init()
        {
            client = new BitBayClient(new ClientConfiguration());
        }

        [TestMethod]
        public void GetInfoAsync_ReturnsProperData()
        {
            
        }
    }
}
