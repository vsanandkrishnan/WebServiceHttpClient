using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
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
            LaptopDetails data = dotNetXmlDesrializer.Deserialize<LaptopDetails>(restResponse);

            if (restResponse.IsSuccessful)
            {
                Console.WriteLine("The status code is " + restResponse.StatusCode);
                Console.WriteLine("The size of the list is " + data.Laptop.Count);
                Assert.AreEqual(200, (int)restResponse.StatusCode);

                Assert.AreEqual("Alienware", data.Laptop[0].BrandName);
                Assert.IsTrue(data.Laptop[0].Features.Feature.Contains("8th Generation Intel® Core™ i5-8300H"));

            }
            else
            {
                Console.WriteLine("Error msg " + restResponse.ErrorMessage);
                Console.WriteLine("Stack trace " + restResponse.ErrorException);
            }

        }


    }
}
