using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Model.JsonModel;
using WebServiceAutomation.Model.XMLModel;

namespace RestSharpAutomation.QueryParameter
{
    [TestClass]
    public class QueryParameter
    {
        private string searchUrl = "http://localhost:8080/laptop-bag/webapi/api/query";

        [TestMethod]
        public void TestQueryParameter()
        {
            var restClient = new RestClient();
            var restRequest = new RestRequest()
            {
                Resource = searchUrl
            };

            //restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Accept", "application/xml");

            // restRequest.AddParameter("id", 1);
            //restRequest.AddHeader("laptopName", "Alienware M17");

            restRequest.AddQueryParameter("id", "1");
            restRequest.AddQueryParameter("laptopName", "Alienware M17");

            var response=restClient.Get<Laptop>(restRequest);

            Assert.AreEqual(200, (int)response.StatusCode);
            Assert.AreEqual("Alienware", response.Data.BrandName);


        }

    }
}
