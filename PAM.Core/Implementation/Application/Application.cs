using System;
using System.Windows.Media;
using System.Xml.Serialization;
using PAM.Core.Abstract;

namespace PAM.Core.Implementation.Application
{

    public class Application : IApplication
    {
        public Application(ApplicationUsages usage = null)
        {

            Usage = usage ?? new ApplicationUsages();

        }

        public Application()
        {
            Usage = new ApplicationUsages();
        }

        public Application(string name, string path)
        {
            Path = path;
            Name = name;
            Usage = new ApplicationUsages();
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
            get { return Usage.TotalUsageTime(); }
        }


        public ApplicationUsages Usage
        {
            get;
            set;
        }

        public ImageSource Icon { get; set; }

        public ApplicationDetails Details { get; set; }
        
        
    }
}
