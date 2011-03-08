using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Media;
using System.Windows.Threading;
using PAM.Core.Implementation.ApplicationImp;

namespace PAM.Core.Implementation.Monitor
{
    public class AppMonitor : INotifyPropertyChanged
    {
        private readonly Timer _timer;
        private readonly AppUpdater _appUpdater;
        private string _currentApplicationName;
        private TimeSpan _currentApplicationTotalUsageTime;
        private string _currentApplicationPath;
        private ImageSource _currentApplicationIcon;
        public Applications Applications
        {
            get { return Data; }
            set
            {
                Data = value;
                AppUpdater.Applications = value;
            }
        }

        public AppMonitor(Dispatcher dispatcher)
        {
            _timer = new Timer { Interval = 1000 };
            _timer.Elapsed += TimerElapsed;
            Data = new Applications();
            _appUpdater = new AppUpdater(Data, dispatcher);

            _timer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            // turn the timer off while processing elapsed because we want to avoid problems with threads - which are not needed here
            var handle = GetForegroundWindow();
            int processId;
            //todo write result to trace and add try catch
            GetWindowThreadProcessId(new HandleRef(null, handle), out processId);
            var process = Process.GetProcessById(processId);

            // checking if the user is in iddle mode - if so, dont updat process
            // todo refactor
            var inputInfo = new LASTINPUTINFO();
            inputInfo.cbSize = (uint)Marshal.SizeOf(inputInfo);
            GetLastInputInfo(ref inputInfo);
            var iddleTime = (Environment.TickCount - inputInfo.dwTime) / 1000;
            if (iddleTime < 30)
            { // iddle time is less 30 sec then update process
                
                var currentApplication = _appUpdater.Update(process);
                if (currentApplication != null)
                {
                    CurrentApplicationName = currentApplication.Name;
                    CurrentApplicationTotalUsageTime = currentApplication.TotalUsageTime;
                    CurrentApplicationPath = currentApplication.Path;
                    CurrentApplicationIcon = currentApplication.Icon;
                }


            }
            else
            {
                _appUpdater.Stop(process);
            }

            _timer.Start();
        }



        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [StructLayout(LayoutKind.Sequential)]
        internal struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }


        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);

        public Applications Data { get; private set; }


        public string CurrentApplicationName
        {
            get { return _currentApplicationName; }
            private set
            {
                if (value == null || value == _currentApplicationName) return;
                _currentApplicationName = value;
                NotifyPropertyChanged("CurrentApplicationName");
            }
        }


        public TimeSpan CurrentApplicationTotalUsageTime
        {
            get { return _currentApplicationTotalUsageTime; }
            private set
            {
                if (value == _currentApplicationTotalUsageTime) return;
                _currentApplicationTotalUsageTime = value;
                NotifyPropertyChanged("CurrentApplicationTotalUsageTime");
            }
        }

        public string CurrentApplicationPath
        {
            get { return _currentApplicationPath; }
            private set
            {
                if (value == _currentApplicationPath) return;
                _currentApplicationPath = value;
                NotifyPropertyChanged("CurrentApplicationPath");
            }
        }



        public ImageSource CurrentApplicationIcon
        {
            get { return _currentApplicationIcon; }
            private set
            {
                if (value == _currentApplicationIcon) return;
                _currentApplicationIcon = value;
                NotifyPropertyChanged("CurrentApplicationIcon");
            }
        }
    }
}