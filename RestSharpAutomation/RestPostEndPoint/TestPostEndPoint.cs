using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.HelperClass.Request;
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


        }

        private ResponseV2Json GetLaptopObject()
        {
            ResponseV2Json laptop = new ResponseV2Json();
            laptop.BrandName = "Dell Inspirion";
            laptop.LaptopName = "Dell Latitude Inspirion";

            Features features = new Features();

            List<string> featureList = new List<string>()
            {
                ("Simple Feature one")
            };

            features.Feature = featureList;

            laptop.Features = features;

            laptop.Id = random.Next(1000);

            return laptop;
        }

        [TestMethod]
        public void TestPostWithModelObject()
        {
            var restClient = new RestClient();
            var restRequest = new RestRequest()
            {
                Resource = postUrl
            };

            restRequest.AddHeader("Content-Type", JsonMediaType);
            restRequest.AddHeader("Accept", JsonMediaType);
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddBody(GetLaptopObject());

            var restResponse = restClient.Post(restRequest);
            

            Assert.AreEqual(200, (int)restResponse.StatusCode);
        }

        [TestMethod]
        public void TestPostWithModelObject_Helper()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {"Content-Type",JsonMediaType},
                {"Accept",JsonMediaType }
            };

            RestClientHelper restClietHelper = new RestClientHelper();

            var restData=restClietHelper.PerformPostRequest<List<ResponseV2Json>>(postUrl,headers,GetLaptopObject(),
                DataFormat.Json);

            


            Assert.AreEqual(200, (int)restData.StatusCode);
        }


    }
}
