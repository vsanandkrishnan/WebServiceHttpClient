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
    public class JiraApiEndtoEndFlow
    {
        private static string JiraBaseUrl = "http://localhost:8080";
        private static string LoginEndPoint = "/rest/auth/1/session";
        private static string LogoutEndPoint = "/rest/auth/1/session";
        private static string CreateProjectEndPoint = "/rest/api/2/project";

        private static IRestClient restClient;
        private static IRestResponse<LoginResponse> loginResponseOutput;


        ///<summary>
        ///1.Login to JIRA -- SessionID -- Test Initialize
        ///2.Create a project--Goes into the test method
        ///3.Logout from Jira -- Class CleanUp
        ///</summary>

        [ClassInitialize]
        public static void LoginInitialize(TestContext context)
        {
            restClient = new RestClient()
            {
                BaseUrl = new Uri(JiraBaseUrl)
            };

            IRestRequest request = new RestRequest()
            {
                Resource = LoginEndPoint
            };

            Login jiraLogin = new Login()
            {
                username = "anandkrishnanvs5",
                password = "Anand@007"
            };
            
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(jiraLogin);

            loginResponseOutput = restClient.Post<LoginResponse>(request);
            Assert.AreEqual(200, (int)loginResponseOutput.StatusCode, $"{loginResponseOutput.StatusCode} is the status returned for login");
        }

        [ClassCleanup]
        public static void LogOut()
        {

            IRestRequest request = new RestRequest()
            {
                Resource = LogoutEndPoint
            };

            request.AddCookie(loginResponseOutput.Data.session.name, loginResponseOutput.Data.session.value);
            var logoutResponse=restClient.Delete(request);
            Assert.AreEqual(204, (int)logoutResponse.StatusCode, $"{logoutResponse.StatusCode} status code output");
        }

        [TestMethod]
        public void CreateProject()
        {
            CreateProjectPayload createProjectPayload = new CreateProjectPayload();
            IRestRequest restRequest = new RestRequest()
            {
                Resource = CreateProjectEndPoint
            };

            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddJsonBody(createProjectPayload);
            restRequest.AddCookie(loginResponseOutput.Data.session.name, loginResponseOutput.Data.session.value);
            var response=restClient.Post<CraeteProjectResponse>(restRequest);

            Assert.AreEqual(201, (int)response.StatusCode);
        }

    }

    
}
