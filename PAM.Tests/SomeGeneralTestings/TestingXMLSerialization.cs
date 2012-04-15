using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework;
using PAM.Core.Implementation.ApplicationImp;

namespace PAM.Tests.SomeGeneralTestings
{

    [TestFixture]
    public class TestingXMLSerialization
    {

        private Applications _applications;

        [SetUp]
        public void Setup()
        {
            _applications = new Applications
                                {
                                    new Application
                                        {
                                            Name = "App1",
                                            Path = @"c:\program files\app1",
                                            Usage = new ApplicationUsages
                                                        {
                                                            new ApplicationUsage(new DateTime(2010,12,01,09,10,20),new DateTime(2010,12,01,09,20,20)),
                                                            new ApplicationUsage(new DateTime(2010,12,01,09,10,20),new DateTime(2010,12,01,09,20,20)),
                                                            new ApplicationUsage(new DateTime(2010,12,01,09,10,20),new DateTime(2010,12,01,09,20,20)),
                                                            new ApplicationUsage(new DateTime(2010,12,01,09,10,20),new DateTime(2010,12,01,09,20,20)),
                                                            new ApplicationUsage(new DateTime(2010,12,01,09,10,20),new DateTime(2010,12,01,09,20,20)),
                                                            new ApplicationUsage(new DateTime(2010,12,01,09,10,20),new DateTime(2010,12,01,09,20,20)),
                                                            new ApplicationUsage(new DateTime(2010,12,01,09,10,20),new DateTime(2010,12,01,09,20,20))
                                                        }
                                        },
                                    new Application
                                        {
                                            Name = "App1",
                                            Path = @"c:\program files\app1",
                                            Usage = new ApplicationUsages
                                                        {
                                                            new ApplicationUsage(new DateTime(2010,12,01,09,10,20),new DateTime(2010,12,01,09,20,20)),
                                                            new ApplicationUsage(new DateTime(2010,12,01,09,10,20),new DateTime(2010,12,01,09,20,20)),
                                                            new ApplicationUsage(new DateTime(2010,12,01,09,10,20),new DateTime(2010,12,01,09,20,20)),
                                                            new ApplicationUsage(new DateTime(2010,12,01,09,10,20),new DateTime(2010,12,01,09,20,20)),
                                                            new ApplicationUsage(new DateTime(2010,12,01,09,10,20),new DateTime(2010,12,01,09,20,20)),
                                                            new ApplicationUsage(new DateTime(2010,12,01,09,10,20),new DateTime(2010,12,01,09,20,20)),
                                                            new ApplicationUsage(new DateTime(2010,12,01,09,10,20),new DateTime(2010,12,01,09,20,20))
                                                        }
                                        },
                                    new Application
                                        {
                                            Name = "App1",
                                            Path = @"c:\program files\app1",
                                            Usage = new ApplicationUsages
                                                        {
                                                            new ApplicationUsage(new DateTime(2010,12,01,09,10,20),new DateTime(2010,12,01,09,20,20)),
                                                            new ApplicationUsage(new DateTime(2010,12,01,09,10,20),new DateTime(2010,12,01,09,20,20)),
                                                            new ApplicationUsage(new DateTime(2010,12,01,09,10,20),new DateTime(2010,12,01,09,20,20)),
                                                            new ApplicationUsage(new DateTime(2010,12,01,09,10,20),new DateTime(2010,12,01,09,20,20)),
                                                            new ApplicationUsage(new DateTime(2010,12,01,09,10,20),new DateTime(2010,12,01,09,20,20)),
                                                            new ApplicationUsage(new DateTime(2010,12,01,09,10,20),new DateTime(2010,12,01,09,20,20)),
                                                            new ApplicationUsage(new DateTime(2010,12,01,09,10,20),new DateTime(2010,12,01,09,20,20))
                                                        }
                                        }

                                };
        }

        [Test]
        public void FirstTest()
        {
            var serializer = new XmlSerializer(typeof(Applications), new XmlRootAttribute("Applications"));
            var inMemoryStream = new MemoryStream();
            serializer.Serialize(inMemoryStream, _applications);
            var xmlTextWriter = new XmlTextWriter(inMemoryStream, Encoding.UTF8);
            inMemoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            var result = Utf8ByteArrayToString(inMemoryStream.ToArray());
            inMemoryStream.Close();

        }

        private static String Utf8ByteArrayToString(Byte[] characters)
        {
            var encoding = new UTF8Encoding();
            var constructedString = encoding.GetString(characters);
            return (constructedString);
        }

    }
}
