using Needletail.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Dynamic;

namespace Needletail.Tests
{
    
    
    /// <summary>
    ///This is a test class for ClientCallTest and is intended
    ///to contain all ClientCallTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ClientCallTest
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
        ///A test for ClientCall Constructor
        ///</summary>
        [TestMethod()]
        public void ClientCallConstructorTest()
        {
            dynamic target = new ClientCall { CallerId = "Test", ClientId = "Test" };
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for GetParameters
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Needletail.Mvc.dll")]
        public void GetParametersTest()
        {
            ClientCall_Accessor target = new ClientCall_Accessor(); // TODO: Initialize to an appropriate value
            object[] expected = null; // TODO: Initialize to an appropriate value
            object[] actual;
            actual = target.GetParameters();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            ClientCall target = new ClientCall(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TryGetMember
        ///</summary>
        [TestMethod()]
        public void TryGetMemberTest()
        {
            ClientCall target = new ClientCall(); // TODO: Initialize to an appropriate value
            GetMemberBinder binder = null; // TODO: Initialize to an appropriate value
            object result = null; // TODO: Initialize to an appropriate value
            object resultExpected = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.TryGetMember(binder, out result);
            Assert.AreEqual(resultExpected, result);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TryInvokeMember
        ///</summary>
        [TestMethod()]
        public void TryInvokeMemberTest()
        {
            ClientCall target = new ClientCall(); // TODO: Initialize to an appropriate value
            InvokeMemberBinder binder = null; // TODO: Initialize to an appropriate value
            object[] args = null; // TODO: Initialize to an appropriate value
            object result = null; // TODO: Initialize to an appropriate value
            object resultExpected = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.TryInvokeMember(binder, args, out result);
            Assert.AreEqual(resultExpected, result);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TrySetMember
        ///</summary>
        [TestMethod()]
        public void TrySetMemberTest()
        {
            ClientCall target = new ClientCall(); // TODO: Initialize to an appropriate value
            SetMemberBinder binder = null; // TODO: Initialize to an appropriate value
            object value = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.TrySetMember(binder, value);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CallerId
        ///</summary>
        [TestMethod()]
        public void CallerIdTest()
        {
            ClientCall target = new ClientCall(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CallerId = expected;
            actual = target.CallerId;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Child
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Needletail.Mvc.dll")]
        public void ChildTest()
        {
            ClientCall_Accessor target = new ClientCall_Accessor(); // TODO: Initialize to an appropriate value
            ClientCall expected = null; // TODO: Initialize to an appropriate value
            ClientCall actual;
            target.Child = expected;
            actual = target.Child;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ClientId
        ///</summary>
        [TestMethod()]
        public void ClientIdTest()
        {
            ClientCall target = new ClientCall(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ClientId = expected;
            actual = target.ClientId;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Method
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Needletail.Mvc.dll")]
        public void MethodTest()
        {
            ClientCall_Accessor target = new ClientCall_Accessor(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Method = expected;
            actual = target.Method;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Parameters
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Needletail.Mvc.dll")]
        public void ParametersTest()
        {
            ClientCall_Accessor target = new ClientCall_Accessor(); // TODO: Initialize to an appropriate value
            object[] expected = null; // TODO: Initialize to an appropriate value
            object[] actual;
            target.Parameters = expected;
            actual = target.Parameters;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
