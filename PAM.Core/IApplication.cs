using System;

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