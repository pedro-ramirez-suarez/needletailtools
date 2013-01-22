using Needletail.Mvc.Communications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Needletail.Mvc;

namespace Needletail.Tests
{
    
    
    /// <summary>
    ///This is a test class for RemoteExecutionTest and is intended
    ///to contain all RemoteExecutionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RemoteExecutionTest
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


        /// <summary>
        ///A test for RemoteExecution Constructor
        ///</summary>
        [TestMethod()]
        public void RemoteExecutionConstructorTest()
        {
            RemoteExecution target = new RemoteExecution();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for BroadcastExecuteOnClient
        ///</summary>
        [TestMethod()]
        public void BroadcastExecuteOnClientTest()
        {
            ClientCall remoteCall = null; // TODO: Initialize to an appropriate value
            RemoteExecution.BroadcastExecuteOnClient(remoteCall);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ExecuteOnClient
        ///</summary>
        [TestMethod()]
        public void ExecuteOnClientTest()
        {
            ClientCall remoteCall = null; // TODO: Initialize to an appropriate value
            bool raiseEventOnError = false; // TODO: Initialize to an appropriate value
            RemoteExecution.ExecuteOnClient(remoteCall, raiseEventOnError);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
