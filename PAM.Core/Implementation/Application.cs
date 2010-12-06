
using System;

namespace PAM.Core
{
    public class Application : IApplication
    {
        private readonly string _applicationName;
        private readonly string _applicationPath;
        private readonly ApplicationUsages _usage;

        public Application(string applicationName,
                           string applicationPath,
                           ApplicationUsages usage = null)
        {
            _applicationName = applicationName;
            _applicationPath = applicationPath;
            _usage = usage ?? new ApplicationUsages();

        }

        public string Path
        {
            get { return _applicationPath; }
        }

        public string Name
        {
            get { return _applicationName; }
        }

        public TimeSpan TotalUsageTime
        {
            get { return _usage.TotalUsageTime(); }
        }

        public ApplicationUsages Usage
        {
            get { return _usage; }
        }


    }
}
