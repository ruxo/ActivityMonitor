using System;
using System.Xml.Serialization;
using PAM.Core.Abstract;

namespace PAM.Core.Implementation.ApplicationImp
{
    public class ApplicationUsage : IApplicationUsage
    {
        [XmlAttribute] public string DetailedName { get; set; } = string.Empty;

        [XmlAttribute]
        public DateTime BeginTime { get; set; }

        [XmlAttribute]
        public DateTime EndTime { get; set; }

        [XmlAttribute]
        public bool IsClosed { get; set; }

        #region ctors

        public ApplicationUsage(){}
        public ApplicationUsage(string detailedName)
        {
            DetailedName = detailedName;
        }

        public ApplicationUsage(DateTime beginTime, DateTime endTime, string detailedName = "")
        {
            BeginTime    = beginTime;
            EndTime      = endTime;
            DetailedName = detailedName;
        }

        #endregion

        public static ApplicationUsage Start(string detailedName) => new(DateTime.Now, DateTime.MinValue, detailedName);

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

        public TimeSpan Total => (IsClosed ? EndTime : DateTime.Now) - BeginTime;
    }
}