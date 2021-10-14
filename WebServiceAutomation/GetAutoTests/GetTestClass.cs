using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceAutomation.GetAutoTests
{
    [TestClass]
    public class GetTestClass
    {
        private string getUrl = @"https://reqres.in/api/users?page=2";
        [TestMethod]
        public void GetMethod()
        {
            HttpClient client = new HttpClient();


            client.GetAsync(getUrl);

            client.Dispose();

        }
    }
}
