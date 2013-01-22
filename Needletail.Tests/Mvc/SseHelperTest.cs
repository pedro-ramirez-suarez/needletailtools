using Needletail.Mvc.Communications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Needletail.Mvc;

namespace Needletail.Tests
{
    
    
    /// <summary>
    ///This is a test class for SseHelperTest and is intended
    ///to contain all SseHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SseHelperTest
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
        ///A test for SseHelper Constructor
        ///</summary>
        [TestMethod()]
        public void SseHelperConstructorTest()
        {
            SseHelper target = new SseHelper();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for AddStream
        ///</summary>
        [TestMethod()]
        public void AddStreamTest()
        {
            string id = string.Empty; // TODO: Initialize to an appropriate value
            StreamWriter streamWriter = null; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = SseHelper.AddStream(id, streamWriter);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for BroadCastMessage
        ///</summary>
        [TestMethod()]
        public void BroadCastMessageTest()
        {
            ClientCall remoteCall = null; // TODO: Initialize to an appropriate value
            SseHelper.BroadCastMessage(remoteCall);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SendMessage
        ///</summary>
        [TestMethod()]
        public void SendMessageTest()
        {
            ClientCall remoteCall = null; // TODO: Initialize to an appropriate value
            bool throwException = false; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = SseHelper.SendMessage(remoteCall, throwException);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
