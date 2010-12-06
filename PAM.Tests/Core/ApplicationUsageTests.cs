using System;
using MbUnit.Framework;
using PAM.Core;
using PAM.Tests.Core;

namespace PAM.Tests.Core
{

    [TestFixture]
    public class ApplicationUsageTests
    {

        private Application _application;

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