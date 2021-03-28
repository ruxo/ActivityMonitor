using System;
using System.Windows.Media;
using PAM.Core.Implementation.ApplicationImp;

namespace PAM.Core.Abstract
{
    public interface IApplication
    {
        string            Path           { get; }
        string            Name           { get; }
        TimeSpan          TotalUsageTime { get; }
        ApplicationUsages Usage          { get; }
        ImageSource?      Icon           { get; set; }
    }
}