using Needletail.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;

namespace Needletail.Tests
{
    
    
    /// <summary>
    ///This is a test class for TwoWayResultTest and is intended
    ///to contain all TwoWayResultTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TwoWayResultTest
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
        ///A test for TwoWayResult Constructor
        ///</summary>
        [TestMethod()]
        public void TwoWayResultConstructorTest()
        {
            ClientCall call = null; // TODO: Initialize to an appropriate value
            TwoWayResult target = new TwoWayResult(call);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for ExecuteResult
        ///</summary>
        [TestMethod()]
        public void ExecuteResultTest()
        {
            ClientCall call = null; // TODO: Initialize to an appropriate value
            TwoWayResult target = new TwoWayResult(call); // TODO: Initialize to an appropriate value
            ControllerContext context = null; // TODO: Initialize to an appropriate value
            target.ExecuteResult(context);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Call
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Needletail.Mvc.dll")]
        public void CallTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            TwoWayResult_Accessor target = new TwoWayResult_Accessor(param0); // TODO: Initialize to an appropriate value
            ClientCall expected = null; // TODO: Initialize to an appropriate value
            ClientCall actual;
            target.Call = expected;
            actual = target.Call;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
