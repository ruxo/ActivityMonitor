
using System;
using System.Xml.Serialization;
using PAM.Core.Implementation;

namespace PAM.Core
{

    public class Application : IApplication
    {
        private readonly ApplicationUsages _usage;

        public Application(
                           ApplicationUsages usage = null)
        {

            _usage = usage ?? new ApplicationUsages();

        }

        public Application()
        {
        }

        public Application(string path,
                           string name)
        {
            Path = path;
            Name = name;
        }

        [XmlAttribute]
        public string Path
        {
            get;
            set;
        }

        [XmlAttribute]
        public string Name { get; set; }

        public TimeSpan TotalUsageTime
        {
            get { return _usage.TotalUsageTime(); }
        }


        public ApplicationUsages Usage
        {
            get;
            set;
        }
    }
}
