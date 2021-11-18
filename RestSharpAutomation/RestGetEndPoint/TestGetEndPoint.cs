using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharp.Authenticators;
using RestSharpAutomation.HelperClass.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Model.JsonModel;
using WebServiceAutomation.Model.XMLModel;

namespace RestSharpAutomation.RestGetEndPoint
{
    [TestClass]
    public class TestGetEndPoint
    {
        private string getUrl = @"http://localhost:8080/laptop-bag/webapi/api/all";
        private string secureGetUrl = @"http://localhost:8080/laptop-bag/webapi/secure/all";
        private static string JsonMediaType = "application/json";
        private static string XmlMediaType = "application/xml";

        [TestMethod]
        public void TestGetInJsonFormat()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(getUrl);
            restRequest.AddHeader("Accept", JsonMediaType);
            IRestResponse restResponse=restClient.Get(restRequest);

            //Checking the response is successful or not.
            if (restResponse.IsSuccessful)
            {
                Console.WriteLine(restResponse.Content);
            }
        }

        [TestMethod]
        public void TestGetInXmlFormat()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(getUrl);
            restRequest.AddHeader("Accept", XmlMediaType);
            IRestResponse restResponse = restClient.Get(restRequest);

            if (restResponse.IsSuccessful)
            {
                Console.WriteLine(restResponse.Content);
            }
        }

        [TestMethod]
        public void TestGetInJsonFormat_DeserializeJson()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(getUrl);
            restRequest.AddHeader("Accept", JsonMediaType);

            IRestResponse<List<ResponseV2Json>> restResponse=restClient.Get<List<ResponseV2Json>>(restRequest);

            if (restResponse.IsSuccessful)
            {
                Console.WriteLine("The status code is "+restResponse.StatusCode);
                Console.WriteLine("The size of the list is "+restResponse.Data.Count);
                Assert.AreEqual(200, (int)restResponse.StatusCode);
                List<ResponseV2Json> data = restResponse.Data;

               ResponseV2Json jsonResponse= data.Find((x) =>
                {
                    return x.Id ==1;

                });

                Assert.AreEqual("Alienware", jsonResponse.BrandName);
                Assert.IsTrue(jsonResponse.Features.Feature.Contains("8th Generation Intel® Core™ i5-8300H"));
            }
            else
            {
                Console.WriteLine("Error msg "+restResponse.ErrorMessage);
                Console.WriteLine("Stack trace "+restResponse.ErrorException);
            }
        }

        [TestMethod]
        public void TestGetInXmlFormat_DeserializeXml()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(getUrl);
            restRequest.AddHeader("Accept", XmlMediaType);

            var dotNetXmlDesrializer = new RestSharp.Deserializers.DotNetXmlDeserializer();
            IRestResponse restResponse = restClient.Get(restRequest);

            if (restResponse.IsSuccessful)
            {
                LaptopDetails data = dotNetXmlDesrializer.Deserialize<LaptopDetails>(restResponse);
                Console.WriteLine("The status code is " + restResponse.StatusCode);
                Console.WriteLine("The size of the list is " + data.Laptop.Count);
                Assert.AreEqual(200, (int)restResponse.StatusCode);

                Laptop dataChecker = data.Laptop.Find(X =>
                 {
                     return X.Id.Equals("1", StringComparison.OrdinalIgnoreCase);

                 });

                Assert.AreEqual("Alienware", dataChecker.BrandName);
                Assert.IsTrue(dataChecker.Features.Feature.Contains("8th Generation Intel® Core™ i5-8300H"));

            }
            else
            {
                Console.WriteLine("Error msg " + restResponse.ErrorMessage);
                Console.WriteLine("Stack trace " + restResponse.ErrorException);
            }

        }

        [TestMethod]
        public void TestGetWithExecute()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Method = Method.GET,
                Resource = getUrl

            };

            restRequest.AddHeader("Accept", JsonMediaType);

            IRestResponse<List<ResponseV2Json>> restResponse=restClient.Execute<List<ResponseV2Json>>(restRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, restResponse.StatusCode);
            Assert.AreEqual("Alienware", restResponse.Data.First().BrandName);
        }

        [TestMethod]
        public void TestGetWithHelperMethods()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {"Accept",XmlMediaType}
            };

            RestClientHelper clientHelper = new RestClientHelper();

            var restResponse=clientHelper.PerformGetRequest(getUrl, headers);

            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse.Content,"Content is not null or empty");
        }

        [TestMethod]
        public void TestGetWithHelperMethodsWithTypeParameter()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {"Accept",XmlMediaType}
            };

            RestClientHelper clientHelper = new RestClientHelper();

            var restResponse = clientHelper.PerformGetRequest<LaptopDetails>(getUrl, headers);

            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse.Data.Laptop, "Content is not null or empty");
        }

        [TestMethod]
        public void TestGetWithHelperMethodsWithTypeParameterJson()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {"Accept",JsonMediaType}
            };

            RestClientHelper clientHelper = new RestClientHelper();

            var restResponse = clientHelper.PerformGetRequest<List<ResponseV2Json>>(getUrl, headers);

            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse.Data.First().BrandName, "Content is not null or empty");
        }

        [TestMethod]
        public void TestGetUsingSecureUrl()
        {
            IRestClient restClient = new RestClient();
            restClient.Authenticator = new HttpBasicAuthenticator("admin", "welcome");
            IRestRequest restRequest = new RestRequest()
            {
                Resource = secureGetUrl
            };

            var restResponse = restClient.Get(restRequest);
            Assert.AreEqual(200, (int)restResponse.StatusCode, "Variable in the method");
        }

    }
}
