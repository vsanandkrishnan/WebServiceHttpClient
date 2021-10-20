using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    }
}
