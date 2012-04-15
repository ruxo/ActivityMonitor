using NUnit.Framework;
using PAM.Core.Implementation.ApplicationImp;

namespace PAM.Tests.Core
{

    [TestFixture]
    public class ApplicationCollectionTests
    {

        private Applications _applications;

        [SetUp]
        public void Setup()
        {
            _applications = new Applications { TestApplication1(), TestApplication2() };
        }

        private static Application TestApplication1()
        {

            return new Application("TestApp1", @"c:\appPath1");
        }

        private static Application TestApplication2()
        {
            return new Application("TestApp2", @"c:\appPath2");
        }

        private static Application TestApplication3()
        {
            return new Application("TestApp3", @"c:\appPath3");
        }

        [Test]
        public void ContainsShoudReturnTrueIfApplicationAlreadyInCollection()
        {
            Assert.IsTrue(_applications.Contains(TestApplication1().Name));
        }

        [Test]
        public void ContainsShouldRetrunTrueIfApplicationAlreadyInCollection2()
        {
            Assert.IsTrue(_applications.Contains(TestApplication1().Name, TestApplication1().Path));
        }

        [Test]
        public void ContainsShoudReturnTrueIfApplicationAlreadyInCollection3()
        {
            Assert.IsTrue(_applications.Contains(TestApplication2().Name));
        }

        [Test]
        public void ContainsShouldRetrunTrueIfApplicationAlreadyInCollection4()
        {
            Assert.IsTrue(_applications.Contains(TestApplication2().Name, TestApplication2().Path));
        }

        [Test]
        public void ContainsShoudReturnFalseIfApplicationIsNotInCollection()
        {
            Assert.IsFalse(_applications.Contains(TestApplication3().Name));
        }

        [Test]
        public void ContainsShoudReturnFalseIfApplicationIsNotInCollection2()
        {
            Assert.IsFalse(_applications.Contains(TestApplication3().Name, TestApplication3().Path));
        }


        [Test]
        public void ContainsShouldRetrunFalseIfCollectionEmpty()
        {
            var collection = new Applications();

            Assert.DoesNotThrow(()=>collection.Contains("cokolwiek"));
            Assert.IsFalse(collection.Contains("cokolwiek"));
        }
    }
}