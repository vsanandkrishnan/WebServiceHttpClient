using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Helper.Request;
using WebServiceAutomation.Helper.Responsedata;
using WebServiceAutomation.Model;
using WebServiceAutomation.Model.XMLModel;

namespace WebServiceAutomation.PutEndPoint
{
    [TestClass]
    public class TestPutEndPoint
    {
        //Post to create an endpoint 
        //Put to update the record
        //Get using id fetch and  Validate the end point

        private string postUrl = @"http://localhost:8080/laptop-bag/webapi/api/add";
        private string getUrl = @"http://localhost:8080/laptop-bag/webapi/api/find/";
        private string putUrl = @"http://localhost:8080/laptop-bag/webapi/api/update";
        private RestResponse restResponse;
        private RestResponse restResponseForGet;
        private static string JsonMediaType = "application/json";
        private static string XmlMediaType = "application/xml";
        private Random random = new Random();


        [TestMethod]
        public void TestPutUsingXmlData()
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

            Dictionary<string, string> httpHeaders = new Dictionary<string, string>
            {
                {"Accept",XmlMediaType }
            };


            restResponse = HttpClientHelper.PerformPostRequest(postUrl,xmlData,XmlMediaType, httpHeaders);
            Assert.AreEqual(200, restResponse.Statuscode, $"{restResponse.ResponseData} is the status code returned while creating the data");

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

            HttpContent httpContent = new StringContent(xmlData, Encoding.UTF8, XmlMediaType);
            restResponse=HttpClientHelper.PerformPutRequest(putUrl, httpContent, httpHeaders);
            Assert.AreEqual(200, restResponse.Statuscode, $"{restResponse.Statuscode} is the status code returned by the rest response");

            restResponse =HttpClientHelper.PerformGetRequest(getUrl + id, httpHeaders);
            Laptop laptop = ResponseDataHelper.DesrializeXmlResponse<Laptop>(restResponse.ResponseData);

            Assert.AreEqual(200, restResponse.Statuscode, $"{restResponse.Statuscode} is the status code returned by the rest response");
            Assert.IsTrue(laptop.Features.Feature.Contains("1TB secondary Storage"), "1TB secondary Storage is not updated in the rest response");





        }
    }
}
