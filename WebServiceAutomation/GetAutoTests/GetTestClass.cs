using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceAutomation.GetAutoTests
{
    [TestClass]
    public class GetTestClass
    {
        private string getUrl = @"http://localhost:8080/laptop-bag/webapi/api/all";
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


    }
}
