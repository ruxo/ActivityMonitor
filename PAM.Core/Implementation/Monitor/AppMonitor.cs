using System.ComponentModel;
using System.Timers;
using System.Windows.Data;
using System.Windows.Threading;
using LanguageExt.UnitsOfMeasure;
using PAM.Core.SettingsManager;
using PAM.Core.Tracker;

namespace PAM.Core.Implementation.Monitor
{
    public sealed class AppMonitor
    {
        readonly Timer             timer;
        readonly AppTrackerContext uiContext;
        readonly Dispatcher        dispatcher;

        public ICollectionView SortedData { get; }

        public AppMonitor(AppTrackerContext uiContext, Dispatcher dispatcher)
        {
            this.uiContext  = uiContext;
            this.dispatcher = dispatcher;
            SortedData      = CollectionViewSource.GetDefaultView(uiContext.Tracked);
            SortedData.SortDescriptions.Add(new("TotalUsageTime", ListSortDirection.Descending));

            timer         =  new() {Interval = 1000, AutoReset = false};
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            var currentProcess = Platform.getActiveProcess(Settings.IdleTime.Seconds());

            dispatcher.Invoke(() => AppTracker.update(currentProcess, uiContext));

            timer.Start();
        }
    }
}