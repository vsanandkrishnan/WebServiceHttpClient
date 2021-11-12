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
        private IRestRequest GetRestRequest(string url, Dictionary<string,string> httpHeaders,Method method)
        {
            IRestRequest restRequest = new RestRequest()
            {
                Resource = url,
                Method = method
            };

            foreach(var key in httpHeaders.Keys)
            {
                restRequest.AddHeader(key, httpHeaders[key]);
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
            return restResponse;

        }




    }
}
