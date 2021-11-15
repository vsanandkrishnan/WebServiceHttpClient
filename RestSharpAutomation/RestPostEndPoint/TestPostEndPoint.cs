using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.HelperClass.Request;
using RestSharpAutomation.ObjectRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Model.JsonModel;
using WebServiceAutomation.Model.XMLModel;

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



        [TestMethod]
        [Obsolete]
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
            restRequest.AddBody(RequestCreation.GetLaptopObject(random.Next(1000)));

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

            var restData=restClietHelper.PerformPostRequest<List<ResponseV2Json>>(postUrl,headers, RequestCreation.GetLaptopObject(random.Next(1000)),
                DataFormat.Json);

            Assert.AreEqual(200, (int)restData.StatusCode);
        }

        [TestMethod]
        public void TestPostWithXml()
        {
            int id = random.Next(1000);
            string xmlData = "<Laptop>" +
                                  "<BrandName>Alienware</BrandName>" +
                           "<Features>" +
                                       "<Feature>8th Generation Intel® Core™ i5-8300H</Feature>" +
                                    "<Feature>Windows 10 Home 64-bit English</Feature>" +
                                    "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>" +
                                    "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                           "</Features>" +
                              "<Id>" + id + "</Id>" +
                              "<LaptopName>Alienware M17</LaptopName>" +
                         "</Laptop>";
            IRestClient restClient = new RestClient();

            IRestRequest restRequest = new RestRequest()
            {
                Resource = postUrl
            };

            restRequest.AddHeader("Content-Type", XmlMediaType);
            restRequest.AddHeader("Accept", XmlMediaType);
            restRequest.AddParameter("XmlBody", xmlData, ParameterType.RequestBody);

            IRestResponse<Laptop> restResponse = restClient.Post<Laptop>(restRequest);

            Assert.AreEqual(200, (int)restResponse.StatusCode);
        }


    }
}
