using System;

namespace PAM.Core.Abstract
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