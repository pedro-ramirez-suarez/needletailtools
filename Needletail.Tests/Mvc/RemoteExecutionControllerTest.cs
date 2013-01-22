using Needletail.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.IO;
using System.Net;

namespace Needletail.Tests
{
    
    
    /// <summary>
    ///This is a test class for RemoteExecutionControllerTest and is intended
    ///to contain all RemoteExecutionControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RemoteExecutionControllerTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        internal virtual RemoteExecutionController CreateRemoteExecutionController()
        {
            // TODO: Instantiate an appropriate concrete class.
            RemoteExecutionController target = null;
            return target;
        }

        /// <summary>
        ///A test for Get
        ///</summary>
        [TestMethod()]
        public void GetTest()
        {
            RemoteExecutionController target = CreateRemoteExecutionController(); // TODO: Initialize to an appropriate value
            HttpRequestMessage request = null; // TODO: Initialize to an appropriate value
            HttpResponseMessage expected = null; // TODO: Initialize to an appropriate value
            HttpResponseMessage actual;
            actual = target.Get(request);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        internal virtual RemoteExecutionController_Accessor CreateRemoteExecutionController_Accessor()
        {
            // TODO: Instantiate an appropriate concrete class.
            RemoteExecutionController_Accessor target = null;
            return target;
        }

        /// <summary>
        ///A test for OnStreamAvailable
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Needletail.Mvc.dll")]
        public void OnStreamAvailableTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            RemoteExecutionController_Accessor target = new RemoteExecutionController_Accessor(param0); // TODO: Initialize to an appropriate value
            Stream stream = null; // TODO: Initialize to an appropriate value
            HttpContent headers = null; // TODO: Initialize to an appropriate value
            TransportContext context = null; // TODO: Initialize to an appropriate value
            target.OnStreamAvailable(stream, headers, context);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for RaiseConnectionLostEvent
        ///</summary>
        [TestMethod()]
        public void RaiseConnectionLostEventTest()
        {
            ClientCall call = null; // TODO: Initialize to an appropriate value
            RemoteExecutionController.RaiseConnectionLostEvent(call);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
