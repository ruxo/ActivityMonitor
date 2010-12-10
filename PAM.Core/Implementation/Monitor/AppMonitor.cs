using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Threading;

namespace PAM.Core.Implementation.Monitor
{
    public class AppMonitor
    {
        private Timer _timer;
        private AppUpdater _appUpdater;

        public AppMonitor(Dispatcher dispatcher)
        {

            _timer = new Timer();
            _timer.Interval = 1000; // todo extract to user settings
            _timer.Elapsed += new ElapsedEventHandler(TimerElapsed);
            Data = new Applications();
            _appUpdater = new AppUpdater(Data, dispatcher);

            _timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            // turn the timer off while processing elapsed because we want to avoid problems with threads - which are not needed here
            var handle = GetForegroundWindow();
            int processId;
            //todo write result to trace and add try catch
            var result = GetWindowThreadProcessId(new HandleRef(null, handle), out processId);
            var process = Process.GetProcessById(processId);

            _appUpdater.Update(process);

            _timer.Start();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);

        public Applications Data { get; private set; }
    }
}