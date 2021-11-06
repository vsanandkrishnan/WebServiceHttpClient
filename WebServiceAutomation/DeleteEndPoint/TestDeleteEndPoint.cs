using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Helper.Request;
using WebServiceAutomation.Helper.Responsedata;
using WebServiceAutomation.Model;
using WebServiceAutomation.Model.XMLModel;

namespace WebServiceAutomation.DeleteEndPoint
{
    [TestClass]
    public class TestDeleteEndPoint
    {
        private string postUrl = @"http://localhost:8080/laptop-bag/webapi/api/add";
        private string deleteUrl = @"http://localhost:8080/laptop-bag/webapi/api/delete/";
        private RestResponse restResponse;
        private static string XmlMediaType = "application/xml";
        private Random random = new Random();

        [TestMethod]
        public void TestDelete()
        {
            int id = random.Next(1000);
            int statusCode=AddRecord(id);

            Assert.AreEqual(200, statusCode, $"{statusCode} is the code returned from the AddRecord method");

            using(HttpClient httpClient = new HttpClient())
            {
                Task<HttpResponseMessage> httpResponseMessage= httpClient.DeleteAsync(deleteUrl + id);
                HttpStatusCode httpStatusCode=httpResponseMessage.Result.StatusCode;
                Assert.AreEqual(HttpStatusCode.OK, httpStatusCode, $"{httpStatusCode} is the code returned from the delete request");
                
            }


        }

        public int AddRecord(int id)
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
                {"Accept",XmlMediaType }
            };
            HttpContent httpContent = new StringContent(xmlData, Encoding.UTF8, XmlMediaType);
            restResponse=HttpClientHelper.PerformPostRequest(postUrl, httpContent, httpHeaders);

            Laptop xmlDataResponse = ResponseDataHelper.DesrializeXmlResponse<Laptop>(restResponse.ResponseData);

            return restResponse.Statuscode;

        }


    }
}
