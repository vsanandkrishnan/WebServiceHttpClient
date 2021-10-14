using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MsTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [Ignore]
        public void TestMethod1()
        {
            Console.WriteLine("Test Method one");
        }

        [TestMethod, TestCategory("Smoke")]
        public void TestMethod2()
        {
            Console.WriteLine("Test Method two");
        }

        [TestInitialize]
        public void SetUp()
        {
            Console.WriteLine("This is setup");
        }

        [TestCleanup]
        public void TearDown()
        {
            Console.WriteLine("This is clean up");
        }

        [ClassInitialize]
        public static void ClassSetUp(TestContext testContext)
        {
            Console.WriteLine("Class is set up "+testContext.TestName);
        }

        [ClassCleanup]
        public static void ClassTearDown()
        {
            Console.WriteLine("Class Tear Down ");
        }


    }
}
