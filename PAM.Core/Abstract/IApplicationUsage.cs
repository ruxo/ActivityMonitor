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

    public class ApplicationUsage : IApplicationUsage
    {
        public DateTime BeginTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public void Start()
        {
            BeginTime = DateTime.Now;
            IsClosed = false;
        }

        public void End()
        {
            EndTime = DateTime.Now;
            IsClosed = true;
        }

        public TimeSpan Total
        {
            get
            {
                return IsClosed ? EndTime.Subtract(BeginTime) : DateTime.Now.Subtract(BeginTime);
            }
        }

        public bool IsClosed { get; private set; }

    }
}