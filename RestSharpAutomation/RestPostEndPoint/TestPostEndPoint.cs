using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Model.JsonModel;

namespace RestSharpAutomation.RestPostEndPoint
{


    [TestClass]
    public class TestPostEndPoint
    {
        private string postUrl = @"http://localhost:8080/laptop-bag/webapi/api/add";
        private string getUrl = @"http://localhost:8080/laptop-bag/webapi/api/find/";
        private Random random = new Random();
        private static string JsonMediaType = "application/json";
        private static string XmlMediaType = "application/xml";

        [TestMethod]
        public void TestPostEndPointWithJson()
        {
            int id = random.Next();
            string jsonData = "{\"BrandName\":\"Alienware\",\"Features\":" +
               "{\"Feature\":[\"8th Generation Intelu00ae Coreu2122 i5-8300H\"," +
               "\"Windows 10 Home 64-bit English\"," +
               "\"NVIDIAu00ae GeForceu00ae GTX 1660 Ti 6GB GDDR6\"," +
               "\"8GB, 2x4GB, DDR4, 2666MHz\"]}," +
               "\"Id\":" + id + ",\"LaptopName\":\"Alienware M17\"}";


            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Resource = postUrl
            };

            restRequest.AddHeader("Content-Type", JsonMediaType);
            restRequest.AddHeader("Accept", XmlMediaType);
            restRequest.AddJsonBody(jsonData);
            IRestResponse restResponse=restClient.Post(restRequest);

            Assert.AreEqual(200, (int)restResponse.StatusCode);

            IRestRequest getRestRequest = new RestRequest()
            {
                Resource = getUrl + id
            };

            getRestRequest.AddHeader("Accept", JsonMediaType);

            IRestResponse<List<ResponseV2Json>> getRestResponse = restClient.Execute<List<ResponseV2Json>>(getRestRequest);

            Assert.IsTrue(getRestResponse.IsSuccessful);

            var response = getRestResponse.Data.Find(resp => resp.Id.Equals(id));

            Assert.AreEqual(response.BrandName, "Alienware");

        }
    }
}
