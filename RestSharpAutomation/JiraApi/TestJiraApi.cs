using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.JiraApi.JiraRequestBody;
using RestSharpAutomation.JiraApi.JiraResponseBody;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpAutomation.JiraApi
{
    [TestClass]
    public class TestJiraApi
    {
        private static string JiraBaseUrl = "http://localhost:8080";
        private static  string LoginEndPoint = "/rest/auth/1/session";


        [TestMethod]
        public void TestJiraLogin()
        {
            Login jiraLogin = new Login()
            {
                username = "anandkrishnanvs5",
                password = "Anand@007"
            };

            IRestClient restClient = new RestClient()
            {
                BaseUrl = new Uri(JiraBaseUrl)
            };

            IRestRequest restRequest = new RestRequest()
            {
                Resource = LoginEndPoint
            };
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddBody(jiraLogin);

            var response=restClient.Post<LoginResponse>(restRequest);

            Assert.AreEqual((int)response.StatusCode, 200, $"{response.StatusCode} is the response from login page");


        }
    }
}
