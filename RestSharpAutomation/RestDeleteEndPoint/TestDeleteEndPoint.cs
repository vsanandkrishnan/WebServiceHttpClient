using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.HelperClass.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Model.XMLModel;

namespace RestSharpAutomation.RestDeleteEndPoint
{
    [TestClass]
    public class TestDeleteEndPoint
    {
        private string postUrl = @"http://localhost:8080/laptop-bag/webapi/api/add";
        private string deleteUrl = @"http://localhost:8080/laptop-bag/webapi/api/delete/";
        private static string XmlMediaType = "application/xml";
        private static string JsonMediaType = "application/json";
        private Random random = new Random();

        [TestMethod]
        public void TestDeleteWithJson()
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
                {"Accept",XmlMediaType },
                {"Content-Type", JsonMediaType}
            };

            RestClientHelper restClientHelper = new RestClientHelper();
            var restResponsePost=restClientHelper.PerformPostRequest<Laptop>(postUrl, headers, jsonData, DataFormat.Json);
            Assert.AreEqual(200, (int)restResponsePost.StatusCode);

            headers = new Dictionary<string, string>()
           {
               {"Accept","*/*" }
           };


            var restResponseDelete=restClientHelper.PerformDeleteRequest(deleteUrl+id,headers);

            Assert.AreEqual(200, (int)restResponseDelete.StatusCode);
        }
    }
}
