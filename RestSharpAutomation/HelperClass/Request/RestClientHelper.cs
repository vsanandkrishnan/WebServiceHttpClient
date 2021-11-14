using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpAutomation.HelperClass.Request
{
    public class RestClientHelper
    {

        private static string XmlMediaType = "application/xml";

        /// <summary>
        /// Rest Client
        /// </summary>
        /// <returns>Rest Client</returns>
        private IRestClient GetRestClient()
        {
            IRestClient restClient = new RestClient();
            return restClient;
        }

        /// <summary>
        /// Rest request implementation method
        /// </summary>
        /// <param name="url"></param>
        /// <param name="httpHeaders"></param>
        /// <param name="method"></param>
        /// <returns>Rest Request</returns>
        private IRestRequest GetRestRequest(string url, Dictionary<string,string> headers,Method method,object body=null,
            DataFormat dataFormat=DataFormat.None)
        {
            IRestRequest restRequest = new RestRequest()
            {
                Resource = url,
                Method = method
            };

            if (headers != null)
            {
                foreach (var key in headers.Keys)
                {
                    restRequest.AddHeader(key, headers[key]);
                }
            }

            if (body != null)
            {
                restRequest.RequestFormat = dataFormat;
                restRequest.AddBody(body);
            }
        
            return restRequest;
        }

        /// <summary>
        /// Execute request without particular response class
        /// </summary>
        /// <param name="restRequest"></param>
        /// <returns>response</returns>
        private IRestResponse SendRequest(IRestRequest restRequest)
        {
            IRestClient restClient = GetRestClient();
            IRestResponse restResponse= restClient.Execute(restRequest);
            return restResponse;
        }

        /// <summary>
        /// Execute request with particulat response class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="restRequest"></param>
        /// <returns></returns>
        private IRestResponse<T> SendRequest<T>(IRestRequest restRequest) where T : new()
        {
            IRestClient restClient = GetRestClient();
            IRestResponse<T> restResponse = restClient.Execute<T>(restRequest);

            if (restResponse.ContentType.Equals(XmlMediaType))
            {
                var deserializer = new RestSharp.Deserializers.DotNetXmlDeserializer();
                restResponse.Data = deserializer.Deserialize<T>(restResponse);
            }
            return restResponse;
        }

        /// <summary>
        /// Perform GET request without deserialization
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns>Rest Response</returns>
        public IRestResponse PerformGetRequest(string url, Dictionary<string,string> headers)
        {
            IRestRequest restRequest = GetRestRequest(url, headers, Method.GET);
            IRestResponse restResponse = SendRequest(restRequest);
            return restResponse;
        }


        /// <summary>
        /// Perform GET request with deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns>Deserialized Rest Response</returns>
        public IRestResponse<T> PerformGetRequest<T>(string url, Dictionary<string, string> headers) where T: new()
        {
            IRestRequest restRequest = GetRestRequest(url, headers, Method.GET);
            IRestResponse<T> restResponse = SendRequest<T>(restRequest);
            return restResponse;
        }

        /// <summary>
        /// Perform POST request with type deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="body"></param>
        /// <param name="format"></param>
        /// <returns>REST RESPONSE</returns>
        public IRestResponse<T> PerformPostRequest<T>(string url, Dictionary<string,string> headers,
            object body,DataFormat format) where T : new()
        {
            IRestRequest restRequest= GetRestRequest(url,headers,Method.POST,body,format);
            IRestResponse<T> restResponse = SendRequest<T>(restRequest);
            return restResponse;
        }

        /// <summary>
        /// Perform POST request with type serialization
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="body"></param>
        /// <param name="format"></param>
        /// <returns>REST RESPONSE</returns>
        public IRestResponse PerformPostRequest(string url, Dictionary<string, string> headers,
            object body, DataFormat format)
        {
            IRestRequest restRequest = GetRestRequest(url, headers, Method.POST, body, format);
            IRestResponse restResponse = SendRequest(restRequest);
            return restResponse;
        }



    }
}
