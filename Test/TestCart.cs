using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server.Controllers;
using Server.Data;
using Server.DI;
using System.Collections.Generic;

namespace Test
{
    [TestClass]
    public class TestCart
    {
        private Server.DI.ITrafficLock service;

        [TestMethod]
        public void TestMethod1()
        {
            service = new FakeService();
            var cart = new CartController(service);
            //var a = new FakeDBContext() as AppDbContext;
            //cart.GetAsync();
        }
    }

    public class FakeService : ITrafficLock
    {
        public Dictionary<string, string> TrafficLock { get; set; } = new Dictionary<string, string>();
    }
}
