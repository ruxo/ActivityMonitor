using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Xml.Serialization;
using PAM.Core.Abstract;

namespace PAM.Core.Implementation.ApplicationImp
{
    public class Application : IApplication, INotifyPropertyChanged
    {
        [XmlAttribute] public string Path { get; set; } = string.Empty;
        [XmlAttribute] public string Name { get; set; } = string.Empty;

        public TimeSpan TotalUsageTime => Usage.TotalUsageTime();

        public double TotalTimeInMunites => TotalUsageTime.TotalMinutes;

        public ApplicationUsages Usage { get; init; }

        [XmlIgnore]
        public ImageSource? Icon { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        #region ctors

        public Application(ApplicationUsages? usage = null)
        {
            Usage = usage ?? new ();
        }

        public Application()
        {
            Usage = new ();
        }

        public Application(string name, string path)
        {
            Path  = path;
            Name  = name;
            Usage = new ();
        }

        #endregion

        public void Refresh()
        {
            NotifyPropertyChanged("TotalUsageTime");
        }

        void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
