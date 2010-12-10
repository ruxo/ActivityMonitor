﻿using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Xml.Serialization;
using PAM.Core.Abstract;

namespace PAM.Core.Implementation.Application
{

    public class Application : IApplication, INotifyPropertyChanged, IObservable<Application>
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public void Refresh()
        {
            NotifyPropertyChanged("TotalTimeInMunites");
        }


        public Application(ApplicationUsages usage = null)
        {

            Usage = usage ?? new ApplicationUsages();

        }

        public Application()
        {
            Usage = new ApplicationUsages();
        }

        public Application(string name, string path)
        {
            Path = path;
            Name = name;
            Usage = new ApplicationUsages();
        }


        [XmlAttribute]
        public string Path
        {
            get;
            set;
        }

        [XmlAttribute]
        public string Name { get; set; }

        public TimeSpan TotalUsageTime
        {
            get { return Usage.TotalUsageTime(); }
        }

        public double TotalTimeInMunites { get { return TotalUsageTime.TotalMinutes; } }

        private ApplicationUsages _usage;

        public ApplicationUsages Usage
        {
            get { return _usage; }
            set
            {
                _usage = value;
                NotifyPropertyChanged("Usage");
            }
        }

        public ImageSource Icon { get; set; }

        public ApplicationDetails Details { get; set; }


        public IDisposable Subscribe(IObserver<Application> observer)
        {
            throw new NotImplementedException();
        }
    }
}
