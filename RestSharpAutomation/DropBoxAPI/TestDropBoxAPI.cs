using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.DropBoxAPI.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpAutomation.DropBoxAPI
{
    [TestClass]
    public class TestDropBoxAPI
    {
        private const string ListEndPointUrl = "https://api.dropboxapi.com/2/files/list_folder";
        private const string AccessToken = "sl.A8of4bo9Zv-C0_j1o9QzxHJuOnGBm-T9cd7lOhkntjX6vQPyZST4Gd_07Vf1JgRI8b51NGtLcfzFrwi11G0fmGOgknie3uuVcZGbwPCucHEDEHsNs-l21DB9HgDa5RKzERtTSGM";

        [TestMethod]
        public void TestListFolder()
        {
            string jsonBody= "{\"path\": \"\",\"recursive\": " +
                "false,\"include_media_info\": false,\"include_deleted\": false,\"include_has_explicit_shared_members\": " +
                "false,\"include_mounted_folders\": true,\"include_non_downloadable_files\": true}";

            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Resource = ListEndPointUrl
            };

            restRequest.AddHeader("Authorization", "Bearer "+AccessToken);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddBody(jsonBody);

            var restResponse=restClient.Post<Root>(restRequest);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
        }
    }
}
