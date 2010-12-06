using System;

namespace PAM.Core
{
    public interface IApplicationUsage
    {
        DateTime BeginTime { get; }
        DateTime EndTime { get; }
        void Start();
        void End();
        TimeSpan Total { get; }
        bool IsClosed { get; }
    }
}