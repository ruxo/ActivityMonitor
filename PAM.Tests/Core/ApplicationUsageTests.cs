using System;
using NUnit.Framework;
using PAM.Core.Implementation.ApplicationImp;

namespace PAM.Tests.Core
{

    [TestFixture]
    public class ApplicationUsageTests
    {
        Application _application;

        [SetUp]
        public void Setup()
        {
            _application = new Application("TestApp", "");
        }

        [Test]
        public void CreatedApplicationHasZeroUsageTime()
        {

            Assert.AreEqual(TimeSpan.Zero,_application.TotalUsageTime);
        }
    }
}