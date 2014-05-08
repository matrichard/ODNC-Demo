using System;
using System.Net.Http;
using System.Web.Http;
using DemoWebApi.Controllers;
using DemoWebApi.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoWebApi.Tests
{
    [TestClass]
    public class RouteTests
    {
        private const string Root = "http://localhost:50959/api";
        private const string GetAction = "Get";

        [TestMethod]
        public void WhenGetBoardsThenBoardController()
        {
            // ARRANGE
            var request = new HttpRequestMessage(HttpMethod.Get, Root + "/boards");
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            // ACT
            var route = WebApiTester.RequeteRoute(config, request);

            // ASSERT
            Assert.AreSame(typeof(BoardsController), route.Controller);
            Assert.AreEqual(GetAction, route.Action);
        }

        [TestMethod]
        public void WhenGetCardsThenCardsController()
        {
            // ARRANGE
            var request = new HttpRequestMessage(HttpMethod.Get, Root + "/cards");
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            // ACT
            var route = WebApiTester.RequeteRoute(config, request);

            // ASSERT
            Assert.AreSame(typeof(CardsController), route.Controller);
            Assert.AreEqual(GetAction, route.Action);
        }

                [TestMethod]
        public void WhenGetCardsForBoardThenCardsController()
        {
            // ARRANGE
            var query = string.Format("{0}/boards/{1}/cards", Root, Guid.NewGuid());
            var request = new HttpRequestMessage(HttpMethod.Get, query);
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            // ACT
            var route = WebApiTester.RequeteRoute(config, request);

            // ASSERT
            Assert.AreSame(typeof(CardsController), route.Controller);
            Assert.AreEqual(GetAction, route.Action);
        }
    }
}
