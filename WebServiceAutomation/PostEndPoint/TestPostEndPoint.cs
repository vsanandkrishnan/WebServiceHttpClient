using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebServiceAutomation.Helper.Authentication;
using WebServiceAutomation.Helper.Request;
using WebServiceAutomation.Helper.Responsedata;
using WebServiceAutomation.Model;
using WebServiceAutomation.Model.JsonModel;
using WebServiceAutomation.Model.XMLModel;

namespace WebServiceAutomation.PostEndPoint
{
    [TestClass]
    public class TestPostEndPoint
    {

        private string postUrl = @"http://localhost:8080/laptop-bag/webapi/api/add";
        private string getUrl = @"http://localhost:8080/laptop-bag/webapi/api/find/";
        private string deleteUrl = @"http://localhost:8080/laptop-bag/webapi/api/delete/";
        private string securePostUrl = @"http://localhost:8080/laptop-bag/webapi/secure/add";
        private string secureGetUrl = @"http://localhost:8080/laptop-bag/webapi/secure/find/";
        private RestResponse restResponse;
        private RestResponse restResponseForGet;
        private static string JsonMediaType = "application/json";
        private static string XmlMediaType = "application/xml";
        private Random random = new Random();
        private int id;

        [TestInitialize]
        public void TestSetUp()
        {
            id = random.Next(1000);
        }

        [TestMethod]
        public void TestPostEndPointJson()
        {
            //Method - PostAsync
            //Body along with Request
            //Header -info about data format           
            string jsonData = "{\"BrandName\":\"Alienware\",\"Features\":" +
                "{\"Feature\":[\"8th Generation Intelu00ae Coreu2122 i5-8300H\"," +
                "\"Windows 10 Home 64-bit English\"," +
                "\"NVIDIAu00ae GeForceu00ae GTX 1660 Ti 6GB GDDR6\"," +
                "\"8GB, 2x4GB, DDR4, 2666MHz\"]}," +
                "\"Id\":"+id+",\"LaptopName\":\"Alienware M17\"}";

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Accept", JsonMediaType);
                HttpContent httpContent = new StringContent(jsonData,Encoding.UTF8,JsonMediaType);
                Task<HttpResponseMessage> httpResponseMessage=httpClient.PostAsync(postUrl, httpContent);
                HttpStatusCode statusCode = httpResponseMessage.Result.StatusCode;
                HttpContent responseContent= httpResponseMessage.Result.Content;

                string responseData = responseContent.ReadAsStringAsync().Result;


                restResponse = new RestResponse((int)statusCode, responseData);

                Assert.AreEqual(200, restResponse.Statuscode);

                Assert.IsNotNull(restResponse.ResponseData);


                Task<HttpResponseMessage> getResponse=httpClient.GetAsync(getUrl + id);

                restResponseForGet = new RestResponse((int)getResponse.Result.StatusCode, 
                    getResponse.Result.Content.ReadAsStringAsync().Result);


                ResponseV2Json jsonResult= JsonConvert.DeserializeObject<ResponseV2Json>(restResponseForGet.ResponseData);

                Assert.AreEqual(id, jsonResult.Id);

            }

        }

        [TestMethod]
        public void TestPostEndPointXml()
        {
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
            using (HttpClient httpClient= new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Accept", XmlMediaType);
                HttpContent httpContent = new StringContent(xmlData, Encoding.UTF8, XmlMediaType);
                Task<HttpResponseMessage> httpResponseMessage = httpClient.PostAsync(postUrl, httpContent);

                HttpStatusCode statusCode = httpResponseMessage.Result.StatusCode;
                HttpContent responseContent = httpResponseMessage.Result.Content;

                string responseData = responseContent.ReadAsStringAsync().Result;

                restResponse = new RestResponse((int)statusCode, responseData);

                Assert.AreEqual(200, restResponse.Statuscode);


                httpResponseMessage = httpClient.GetAsync(getUrl + id);
                statusCode = httpResponseMessage.Result.StatusCode;
                responseContent = httpResponseMessage.Result.Content;


                restResponseForGet = new RestResponse((int)statusCode, responseContent.ReadAsStringAsync().Result);


                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Laptop));
                TextReader textReader = new StringReader(restResponseForGet.ResponseData);

                Laptop laptop = (Laptop)xmlSerializer.Deserialize(textReader);

                Assert.IsTrue(laptop.Id.Contains(id.ToString()));

            }
        }

        [TestMethod]
        public void TestPostEndPointUsingSendAsync()
        {
            string jsonData = "{\"BrandName\":\"Alienware\",\"Features\":" +
                "{\"Feature\":[\"8th Generation Intelu00ae Coreu2122 i5-8300H\"," +
                "\"Windows 10 Home 64-bit English\"," +
                "\"NVIDIAu00ae GeForceu00ae GTX 1660 Ti 6GB GDDR6\"," +
                "\"8GB, 2x4GB, DDR4, 2666MHz\"]}," +
                "\"Id\":" + id + ",\"LaptopName\":\"Alienware M17\"}";


            using (HttpClient httpClient= new HttpClient())
            {
                using(HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(postUrl);
                    httpRequestMessage.Content = new StringContent(jsonData, Encoding.UTF8, JsonMediaType);
                    Task<HttpResponseMessage> httpResponseMessage = httpClient.SendAsync(httpRequestMessage);

                    restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);

                    Assert.AreEqual(200, restResponse.Statuscode);
                }
            }

        }

        [TestMethod]
        public void TestPostEndPointUsingSendAsyncXml()
        {
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

            using(HttpClient httpClient = new HttpClient())
            {
                using(HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(postUrl);
                    httpRequestMessage.Content = new StringContent(xmlData, Encoding.UTF8, XmlMediaType);

                    Task<HttpResponseMessage> httpResponseMessage = httpClient.SendAsync(httpRequestMessage);

                    restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);

                    Assert.AreEqual(200, restResponse.Statuscode);
                }
            }
        }

        [TestMethod]
        public void TestPostUsingHelperClass()
        {
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

            Dictionary<string, string> httpHeaders = new Dictionary<string, string>()
            {
                {"Accept", "application/xml" }
            };


            restResponse = HttpClientHelper.PerformPostRequest(postUrl, xmlData, XmlMediaType, httpHeaders);

            var xmlResponse=ResponseDataHelper.DesrializeXmlResponse<Laptop>(restResponse.ResponseData);

            Assert.AreEqual("Alienware", xmlResponse.BrandName);
            
            Assert.AreEqual(200, restResponse.Statuscode);
        }

        [TestMethod]
        public void PostUsingSecureUrls()
        {
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

            string auth = Base64StringConverter.GetBase64String("admin", "welcome");
            auth = "Basic " + auth;
            Dictionary<string, string> httpHeaders = new Dictionary<string, string>()
            {
                {"Accept", "application/xml" },
                {"Authorization",auth}
            };

            restResponse = HttpClientHelper.PerformPostRequest(securePostUrl, xmlData, XmlMediaType, httpHeaders);
            Assert.AreEqual(200, restResponse.Statuscode);

            restResponseForGet = HttpClientHelper.PerformGetRequest(secureGetUrl + id,httpHeaders);
            Assert.AreEqual(200, restResponseForGet.Statuscode);



        }



        [TestCleanup]
        public void TearDownMethod()
        {
            restResponse = HttpClientHelper.PerformDeleteRequest(deleteUrl+id);
            Assert.AreEqual(restResponse.Statuscode, 200);
        }

    }
}
