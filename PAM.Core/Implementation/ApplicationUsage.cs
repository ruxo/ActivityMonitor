using System;
using System.Xml.Serialization;

namespace PAM.Core
{
    public class ApplicationUsage : IApplicationUsage
    {
        public ApplicationUsage()
        {
        }

        public ApplicationUsage(DateTime beginTime,
                                DateTime endTime)
        {
            BeginTime = beginTime;
            EndTime = endTime;
        }


        [XmlAttribute]
        public DateTime BeginTime { get; set; }

        [XmlAttribute]
        public DateTime EndTime { get; set; }

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

        [XmlAttribute]
        public bool IsClosed { get; set; }

    }
}