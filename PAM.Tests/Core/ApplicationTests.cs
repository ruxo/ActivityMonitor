using MbUnit.Framework;
using PAM.Core.Implementation.ApplicationImp;

namespace PAM.Tests.Core
{

    [TestFixture]
    public class ApplicationTests
    {

        private Application _application;
        private const string AppPath = @"c:\program files\app\app.exe";
        private const string AppName = "TestApp";

        [SetUp]
        public void Setup()
        {
            _application = new Application("TestApp", AppPath);

        }

        [Test]
        public void NameRetrunsNameOfTheApplication()
        {
            Assert.AreEqual(AppName, _application.Name);
        }

        [Test]
        public void PathReturnPathOfTheApplication()
        {
            Assert.AreEqual(AppPath, _application.Path);
        }
    }
}