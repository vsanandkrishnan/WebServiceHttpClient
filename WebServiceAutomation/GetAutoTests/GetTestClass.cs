using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebServiceAutomation.Helper.Authentication;
using WebServiceAutomation.Helper.Request;
using WebServiceAutomation.Helper.Responsedata;
using WebServiceAutomation.Model;
using WebServiceAutomation.Model.JsonModel;
using WebServiceAutomation.Model.XMLModel;

namespace WebServiceAutomation.GetAutoTests
{
    [TestClass]
    public class GetTestClass
    {
        private string getUrl = @"http://localhost:8080/laptop-bag/webapi/api/all";
        private string secureGetUrl = @"http://localhost:8080/laptop-bag/webapi/secure/all";
        private string delayGet = @"http://localhost:8080/laptop-bag/webapi/delay/all";


        [TestMethod]
        public void GetMethodwithUrl()
        {
           HttpClient client = new HttpClient();
           var response= client.GetAsync(getUrl);
           client.Dispose();
        }

        [TestMethod]
        public void GetMethodwithUri()
        {
            HttpClient client = new HttpClient();
            Uri getUri = new Uri(getUrl);
            Task<HttpResponseMessage> httpResponse = client.GetAsync(getUri);
            HttpResponseMessage httpResponseMessage=httpResponse.Result;
            HttpStatusCode httpStatusCode = httpResponseMessage.StatusCode;
            Console.WriteLine(httpResponseMessage.ToString());

            //Status code
            Console.WriteLine("The status code is "+httpStatusCode);
            Console.WriteLine("The status code is "+(int)httpStatusCode);

            //Http Content
            HttpContent responseContent = httpResponseMessage.Content;
            Task<string> responseData=responseContent.ReadAsStringAsync();
            string data=responseData.Result;
            Console.WriteLine(data);



            client.Dispose();
        }

        [TestMethod]
        public void GetMethodwithInvalidUri()
        {
            HttpClient client = new HttpClient();
            Uri getUri = new Uri(getUrl+"/random/7/k");
            Task<HttpResponseMessage> httpResponse = client.GetAsync(getUri);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;
            HttpStatusCode httpStatusCode = httpResponseMessage.StatusCode;
            Console.WriteLine(httpResponseMessage.ToString());
            Console.WriteLine("The status code is " + httpStatusCode);
            Console.WriteLine("The status code is " + (int)httpStatusCode);
            client.Dispose();
        }

        [TestMethod]
        public void TestGetAllEndPointInJsonFormat()
        {
            HttpClient client = new HttpClient();
            HttpRequestHeaders headers=client.DefaultRequestHeaders;
            headers.Add("Accept", "application/json");

            Uri getUri = new Uri(getUrl);
            Task<HttpResponseMessage> httpResponseMessage = client.GetAsync(getUri);
            HttpResponseMessage httpResponse = httpResponseMessage.Result;
            HttpStatusCode httpStatusCode = httpResponse.StatusCode;

            //Status code
            Console.WriteLine("The status code is " + httpStatusCode);
            Console.WriteLine("The status code is " + (int)httpStatusCode);

            //Http Content
            HttpContent responseContent = httpResponse.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            client.Dispose();
        }

        [TestMethod]
        public void TestGetAllEndPointInXmlFormat()
        {
            HttpClient client = new HttpClient();
            HttpRequestHeaders httpRequestHeaders = client.DefaultRequestHeaders;
            httpRequestHeaders.Add("Accept", "application/xml");

            Uri getUri = new Uri(getUrl);
            Task<HttpResponseMessage> httpResponseMessage = client.GetAsync(getUri);
            HttpResponseMessage httpResponse=httpResponseMessage.Result;
            HttpStatusCode httpStatusCode = httpResponse.StatusCode;

            //Status Code
            Console.WriteLine("The status Code is => "+httpStatusCode );
            Console.WriteLine("The Status code is => "+(int)httpStatusCode);

            //Response Body
            HttpContent httpContent = httpResponse.Content;
            Task<string> responseData = httpContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            client.Dispose();           
        }

        [TestMethod]
        public void TestSendAsyncMethod()
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.RequestUri = new Uri(getUrl);
            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.Headers.Add("Accept", "application/json");

            HttpClient client = new HttpClient();
            Task<HttpResponseMessage> httpResponseMessage=client.SendAsync(httpRequestMessage);

            HttpResponseMessage httpResponse = httpResponseMessage.Result;
            HttpStatusCode statusCode = httpResponse.StatusCode;

            Console.WriteLine((int)statusCode);

            HttpContent content = httpResponse.Content;
            Task<string> responseData = content.ReadAsStringAsync();

            string data = responseData.Result;

            Console.WriteLine(data);

        }

        [TestMethod]
        public void TestUsingStatement()
        {
            using (HttpClient httpClient= new HttpClient())
            {
                using(HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(getUrl);
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.Headers.Add("Accept", "application/xml");

                    Task<HttpResponseMessage> httpResponseMessage = httpClient.SendAsync(httpRequestMessage);
                    using (HttpResponseMessage httpResponse = httpResponseMessage.Result)
                    {
                        HttpStatusCode statusCode = httpResponse.StatusCode;

                        Console.WriteLine((int)statusCode);

                        HttpContent httpContent = httpResponse.Content;

                        Task<string> response = httpContent.ReadAsStringAsync();

                        string responseOut = response.Result;

                        Console.WriteLine(responseOut);

                        RestResponse responseV = new RestResponse((int)statusCode, responseOut);

                        string output=responseV.ToString();

                        Console.WriteLine(output);

                    }
                }

            }
        }

        [TestMethod]
        public void TestUsingDeserializationOfJson()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(getUrl);
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    Task<HttpResponseMessage> httpResponseMessage = httpClient.SendAsync(httpRequestMessage);
                    using (HttpResponseMessage httpResponse = httpResponseMessage.Result)
                    {
                        HttpStatusCode statusCode = httpResponse.StatusCode;

                        Console.WriteLine((int)statusCode);

                        HttpContent httpContent = httpResponse.Content;

                        Task<string> response = httpContent.ReadAsStringAsync();

                        string responseOut = response.Result;

                        Console.WriteLine(responseOut);

                        RestResponse responseV = new RestResponse((int)statusCode, responseOut);

                        string output = responseV.ToString();

                        Console.WriteLine(output);

                        List< ResponseV2Json> responseDesialized=JsonConvert.DeserializeObject<List<ResponseV2Json>>(responseV.ResponseData);

                        Console.WriteLine(responseDesialized[0].LaptopName);

                    }
                }
            }
        }


        [TestMethod]
        public void TestUsingXMLDeserialization()
        {

            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(getUrl);
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.Headers.Add("Accept", "application/xml");

                    Task<HttpResponseMessage> httpResponseMessage = httpClient.SendAsync(httpRequestMessage);
                    using (HttpResponseMessage httpResponse = httpResponseMessage.Result)
                    {
                        HttpStatusCode statusCode = httpResponse.StatusCode;

                        Console.WriteLine((int)statusCode);

                        HttpContent httpContent = httpResponse.Content;

                        Task<string> response = httpContent.ReadAsStringAsync();

                        string responseOut = response.Result;

                        Console.WriteLine(responseOut);

                        RestResponse responseV = new RestResponse((int)statusCode, responseOut);

                        string output = responseV.ToString();

                        Console.WriteLine(output);

                        //Step1
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(LaptopDetails));

                        //Step2
                        TextReader textReader = new StringReader(responseV.ResponseData);

                        //Step3
                        LaptopDetails xmlData= (LaptopDetails)xmlSerializer.Deserialize(textReader);

                        //1st checkpoint

                        Assert.AreEqual(200, responseV.Statuscode);

                        //2nd Checkpoint

                       Assert.IsNotNull(responseV.ResponseData);


                        //returns boolean value
                        string feature = "8th Generation Intel® Core™ i5-8300H";
                        var IsPresent = xmlData.Laptop.Any(X => X.Features.Feature.Contains(feature));
                            //Features.Feature.Contains(feature);

                        Assert.IsTrue(IsPresent, $"The features does not contain the feature {feature}");
                    }
                }
            }

        }

        [TestMethod]
        public void GetUsingHelperMethod()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/json");


            var restResponse=HttpClientHelper.PerformGetRequest(getUrl, httpHeader);

            //List<ResponseV2Json> responseV2Json = JsonConvert.DeserializeObject<List<ResponseV2Json>>(restResponse.ResponseData);

            var responseV2Json = ResponseDataHelper.DeserializeJsonResponse<List<ResponseV2Json>>(restResponse.ResponseData);

            Console.WriteLine(responseV2Json.First().BrandName);

        }


        [TestMethod]
        public void GetUsingSecureUrl()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/json");
            // httpHeader.Add("Authorization", "Basic YWRtaW46d2VsY29tZQ==");

            string authHeader =Base64StringConverter.GetBase64String("admin", "welcome");

            authHeader = "Basic " + authHeader;

            httpHeader.Add("Authorization", authHeader);

            var restResponse = HttpClientHelper.PerformGetRequest(secureGetUrl, httpHeader);


            var responseV2Json = ResponseDataHelper.DeserializeJsonResponse<List<ResponseV2Json>>(restResponse.ResponseData);
            Assert.AreEqual(200, restResponse.Statuscode);


        }

        [TestMethod]
        public void GetDelayGetRequest()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/json");


            RestResponse restResponse=HttpClientHelper.PerformGetRequest(@"http://localhost:8080/laptop-bag/webapi/delay/all", httpHeader);

            Assert.AreEqual(200, restResponse.Statuscode);
        }

        [TestMethod]
        public void GetDelayGetRequest_Async()
        {
            Task t1 = new Task(GetEndPoint());
            t1.Start();
            Task t2 = new Task(GetEndPoint());
            t2.Start();
            Task t3 = new Task(GetEndPoint());
            t3.Start();
            Task t4 = new Task(GetEndPointFailed());
            t4.Start();

            t1.Wait();
            t2.Wait();
            t3.Wait();
            t4.Wait();

        }

        private Action GetEndPoint()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/xml");
            return new Action(() =>
            {
                RestResponse restResponse=HttpClientHelper.PerformGetRequest(delayGet, httpHeader);
                Assert.AreEqual(200, restResponse.Statuscode);
            });
        }

        private Action GetEndPointFailed()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/xml");
            return new Action(() =>
            {
                RestResponse restResponse = HttpClientHelper.PerformGetRequest(delayGet, httpHeader);
                Assert.AreEqual(201, restResponse.Statuscode);
            });
        }

    }
}
