using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.HelperClass.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Model.JsonModel;
using WebServiceAutomation.Model.XMLModel;

namespace RestSharpAutomation.RestPutEndPoint
{
    [TestClass]
    public class TestPutEndPoint
    {
        private string postUrl = @"http://localhost:8080/laptop-bag/webapi/api/add";
        private string getUrl = @"http://localhost:8080/laptop-bag/webapi/api/find/";
        private string putUrl = @"http://localhost:8080/laptop-bag/webapi/api/update";

        private static string JsonMediaType = "application/json";
        private static string XmlMediaType = "application/xml";
        private Random random = new Random();

        [TestMethod]
        public void TestPutWithJsonData()
        {
            int id = random.Next(1000);
            string jsonData = "{\"BrandName\":\"Alienware\",\"Features\":" +
               "{\"Feature\":[\"8th Generation Intelu00ae Coreu2122 i5-8300H\"," +
               "\"Windows 10 Home 64-bit English\"," +
               "\"NVIDIAu00ae GeForceu00ae GTX 1660 Ti 6GB GDDR6\"," +
               "\"8GB, 2x4GB, DDR4, 2666MHz\"]}," +
               "\"Id\":" + id + ",\"LaptopName\":\"Alienware M17\"}";

            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {"Accept",XmlMediaType},
                {"Content-Type",JsonMediaType}
            };

            RestClientHelper restClientHelper = new RestClientHelper();

            IRestResponse<Laptop> restResponse=restClientHelper.PerformPostRequest<Laptop>(postUrl, headers, jsonData, DataFormat.Json);
            Assert.IsTrue(restResponse.IsSuccessful,"The response is not successful");

            jsonData = "{\"BrandName\":\"Alienware\",\"Features\":" +
               "{\"Feature\":[\"8th Generation Intelu00ae Coreu2122 i5-8300H\"," +
               "\"Windows 10 Home 64-bit English\"," +
               "\"NVIDIAu00ae GeForceu00ae GTX 1660 Ti 6GB GDDR6\"," +
               "\"8GB, 2x4GB, DDR4, 2666MHz\"," +
               "\"New Feature Added\"]}," +
               "\"Id\":" + id + ",\"LaptopName\":\"Alienware M17\"}";

            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Resource = putUrl
            };

            restRequest.AddHeader("Accept", JsonMediaType);
            restRequest.AddHeader("Content-Type", JsonMediaType);
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddJsonBody(jsonData);

            IRestResponse<List<ResponseV2Json>> restResponsePut=restClient.Put<List<ResponseV2Json>>(restRequest);

            Assert.IsTrue(restResponsePut.Data.FindAll(x => x.Id == id).First().Features.Feature.Contains("New Feature Added"),"The new festure was not added to the Json");

            headers = new Dictionary<string, string>()
            {
                {"Accept",JsonMediaType }
            };

            var restResponseGet=restClientHelper.PerformGetRequest<List<ResponseV2Json>>(getUrl, headers);

            Assert.AreEqual(200, (int)restResponseGet.StatusCode,$"{restResponseGet.StatusCode} was returned from get request");

            var featuresInJson = restResponseGet.Data.Find(X => X.Id == id).Features.Feature;
            Assert.IsTrue(featuresInJson.Contains("New Feature Added"),"Feature got added in Json");

            Console.WriteLine(restResponseGet.Data.Count);
        }

        [TestMethod]
        public void TestPutWithXMl()
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

            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {"Accept",XmlMediaType},
                {"Content-Type",XmlMediaType}
            };

            RestClientHelper restClientHelper = new RestClientHelper();
            var restResponsePost=restClientHelper.PerformPostRequest<Laptop>(postUrl, headers, xmlData, DataFormat.Xml);
            Assert.AreEqual(200, (int)restResponsePost.StatusCode,$"{restResponsePost.StatusCode} is the response code that was like output");

            xmlData = "<Laptop>" +
                      "<BrandName>Alienware</BrandName>" +
               "<Features>" +
                           "<Feature>8th Generation Intel® Core™ i5-8300H</Feature>" +
                        "<Feature>Windows 10 Home 64-bit English</Feature>" +
                        "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>" +
                        "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                        "<Feature>1TB secondary Storage</Feature>" +
               "</Features>" +
                  "<Id>" + id + "</Id>" +
                  "<LaptopName>Alienware M17</LaptopName>" +
             "</Laptop>";

            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Resource = putUrl
            };

            restRequest.AddHeader("Accept", XmlMediaType);
            restRequest.AddHeader("Content-Type", XmlMediaType);
            restRequest.RequestFormat = DataFormat.Xml;

            restRequest.AddParameter("XmlBody", xmlData, ParameterType.RequestBody);

            var restResponsePut = restClient.Put(restRequest);
            Assert.AreEqual(200, (int)restResponsePut.StatusCode);

            var deserializer = new RestSharp.Deserializers.DotNetXmlDeserializer();
            var restResponseDeserialized = deserializer.Deserialize<Laptop>(restResponsePut);

            var featureUpdatedOne = restResponseDeserialized.Features.Feature;
            Assert.IsTrue(featureUpdatedOne.Contains("1TB secondary Storage"),"The new feature was not updated in Xml file");

            headers = new Dictionary<string, string>()
            {
                {"Accept",XmlMediaType }
            };

            var restResponseGet=restClientHelper.PerformGetRequest<Laptop>(getUrl+id, headers);
            Assert.AreEqual(200, (int)restResponseGet.StatusCode,$"{restResponseGet.StatusCode} is send out after the Get request");



        }

    }
}
