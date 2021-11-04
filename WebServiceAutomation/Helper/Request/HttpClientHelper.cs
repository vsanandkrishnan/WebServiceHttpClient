using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Model;

namespace WebServiceAutomation.Helper.Request
{
     class HttpClientHelper
    {
        private static HttpClient httpClient;
        private static HttpRequestMessage httpRequestMessage;
        private static RestResponse restResponse;


        /// <summary>
        /// Http Client Method
        /// </summary>
        /// <param name="httpHeaders"></param>
        /// <returns></returns>
        private static HttpClient AddHeadersAndCreateHttpClient(Dictionary<string,string> httpHeaders)
        {
            HttpClient httpClient = new HttpClient();

            if (httpHeaders != null)
            {
                foreach (string key in httpHeaders.Keys)
                {
                    httpClient.DefaultRequestHeaders.Add(key, httpHeaders[key]);
                }
            }
            

            return httpClient;
        }

        /// <summary>
        /// Http Create Request Message
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <param name="httpMethod"></param>
        /// <param name="httpContent"></param>
        /// <returns></returns>
        private static HttpRequestMessage CreateHttpRequestMessage(string requestUrl, HttpMethod httpMethod, HttpContent httpContent)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpMethod, requestUrl);

            if(httpMethod==HttpMethod.Post)
                httpRequestMessage.Content = httpContent;

            return httpRequestMessage;
        }

        /// <summary>
        /// Resposne from APIs
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <param name="httpMethod"></param>
        /// <param name="httpContent"></param>
        /// <param name="httpHeaders"></param>
        private static RestResponse SendRequest(string requestUrl, HttpMethod httpMethod, HttpContent httpContent,
            Dictionary<string, string> httpHeaders)
        {

            httpClient = AddHeadersAndCreateHttpClient(httpHeaders);
            httpRequestMessage = CreateHttpRequestMessage(requestUrl, httpMethod, httpContent);

            try
            {
                Task<HttpResponseMessage> httpResponseMessage=httpClient.SendAsync(httpRequestMessage);
                restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
            }catch(Exception e)
            {
                restResponse = new RestResponse(500, e.Message);
            }
            finally
            {
                //Null Check Operator
                httpRequestMessage?.Dispose();
                httpClient?.Dispose();

            }

            return restResponse;
        }

        /// <summary>
        /// Get Request 
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <param name="httpHeaders"></param>
        /// <returns></returns>
        public static RestResponse PerformGetRequest(string requestUrl, Dictionary<string, string> httpHeaders)
        {
            return SendRequest(requestUrl, HttpMethod.Get, null, httpHeaders);
        }

        /// <summary>
        /// Direct Post Request when HttpContent is given.
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <param name="httpContent"></param>
        /// <param name="httpHeaders"></param>
        /// <returns></returns>
        public static RestResponse PerformPostRequest(string requestUrl, HttpContent httpContent, 
            Dictionary<string, string> httpHeaders)
        {
            return SendRequest(requestUrl, HttpMethod.Post, httpContent, httpHeaders);
        }

        /// <summary>
        /// Post Request starting from StringContent Data.
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <param name="data"></param>
        /// <param name="mediaType"></param>
        /// <param name="httpHeaders"></param>
        /// <returns></returns>
        public static RestResponse PerformPostRequest(string requestUrl, string data, string mediaType,
            Dictionary<string, string> httpHeaders)
        {
            HttpContent httpContent = new StringContent(data, Encoding.UTF8, mediaType);
            return PerformPostRequest(requestUrl,httpContent, httpHeaders);
        }


    }
}
