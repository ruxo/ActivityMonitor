using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Threading;
using PAM.Core.Implementation.ApplicationImp;

namespace PAM.Core.Implementation.Monitor
{
    public class AppMonitor : INotifyPropertyChanged
    {
        private readonly Timer _timer;
        private readonly AppUpdater _appUpdater;

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

            CurrentApplication = (Application)_appUpdater.Update(process);
            _timer.Start();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);

        public Applications Data { get; private set; }

        private Application _currentApplication;
        public Application CurrentApplication
        {
            get
            {
                return _currentApplication;
            }
            private set
            {
                if (value == null || value == _currentApplication) return;
                _currentApplication = value;
                NotifyPropertyChanged("CurrentApplication");
                Debug.WriteLine("CurrentApplicationChanged");
            }
        }
    }
}