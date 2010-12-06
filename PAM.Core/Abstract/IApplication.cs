using System;
using PAM.Core.Implementation;

namespace PAM.Core
{
    public interface IApplication
    {
        string Path { get; }
        string Name { get; }
        TimeSpan TotalUsageTime { get; }
        ApplicationUsages Usage { get; }

    }
}