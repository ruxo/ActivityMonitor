using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Win32;
using PAM.Core.Extensions;
using PAM.Core.Implementation.Monitor;
using PAM.Utils.Export;
using PAM.Utils.Settings;
using PAM.Utils.VersionChecking;
using PAM.Views;
using PAM.Windows;

namespace PAM
{
    /// <summary>
    /// Interaction logic for MainGraphWindow.xaml
    /// </summary>
    public partial class MainGraphWindow
    {
        WindowState _lastWindowState;
        bool _shouldClose;
        private AppMonitor _monitor;

        public MainGraphWindow()
        {
            InitializeComponent();

            //AutostartMenuItem.Checked = Core.SettingsManager.Settings.Autostart;
            Core.SettingsManager.Settings.SettingsProvider = new SettingsProvider();

            CheckForNewVersion();
        }

        private void CheckForNewVersion(bool showMessageWhenNoNewVersion = false)
        {
            Task.Factory.StartNew(() =>
            {
                var versionChecker = new VersionChecker();
                return versionChecker.GetLatestVersionInfo();
            }).ContinueWith(m =>
            {

                if (m.Result.Version.CompareTo(Assembly.GetExecutingAssembly().GetName().Version.ToString()) > 0)
                {

                    var newVersionView = new NewVersionPopupView();
                    newVersionView.DataContext = m.Result;
                    taskbarIcon.ShowCustomBalloon(newVersionView, PopupAnimation.Slide, 5000);
                }
                else {
                    var noNewVersionView = new NoNewVersionPopupView();
                    taskbarIcon.ShowCustomBalloon(noNewVersionView, PopupAnimation.Slide, 5000);
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());

        }




        protected override void OnStateChanged(EventArgs e)
        {
            _lastWindowState = WindowState;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_shouldClose) return;
            e.Cancel = true;
            Hide();
        }

        private void OnNotificationAreaIconDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Open();
            }
        }

        private void OnMenuItemOpenClick(object sender, EventArgs e)
        {
            Open();
        }

        private void Open()
        {
            Show();
            WindowState = _lastWindowState;
        }

        private void OnMenuItemExitClick(object sender, EventArgs e)
        {
            _shouldClose = true;
            Close();
        }

        private void FormLoaded(object sender, RoutedEventArgs e)
        {
            _monitor = new AppMonitor(Dispatcher);
            appsTree.Applications = _monitor.SortedData as CollectionView;
            CurrentApp.DataContext = _monitor;
            _monitor.PropertyChanged += new PropertyChangedEventHandler(_monitor_PropertyChanged);
        }

        void _monitor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //appsTree.Applications.Refresh();

            this.InvokeIfRequired(() => appsTree.Applications.Refresh());
        }

        //private void SaveButtonClick(object sender, RoutedEventArgs e)
        //{
        //    var persister = new AppMonitorPersister(_monitor.Applications);
        //    persister.Save();
        //}

        //private void ReadButtonClick(object sender, RoutedEventArgs e)
        //{
        //    var persister = new AppMonitorPersister(_monitor.Applications);
        //    _monitor.Applications = persister.Restore();

        //    appsTree.Applications = _monitor.Data;

        //}

        //private void FilterButtonClick(object sender, RoutedEventArgs e)
        //{
        //    var result = from data in _monitor.Data
        //                 where (from details in data.Details
        //                        where details.Usages.Count > 10
        //                        select details) != null
        //                 select data;

        //    appsTree.Applications = result;
        //}


        private void OnMenuItemSettingsClick(object sender, EventArgs e)
        {
            var settingsWindow = new Settings();
            settingsWindow.ShowDialog();
        }

        private void OnMenuItemAboutClick(object sender, EventArgs e)
        {
            var aboutWindow = new AboutBox(null);
            aboutWindow.Show();
        }

        private void OnMenuItemExportClick(object sender,
                                           EventArgs e)
        {


            var saveWindow = new SaveFileDialog
                                 {
                                     FileName =
                                         string.Format("Personal Activity Monitor ({0}).xml",
                                                       DateTime.Now.ToShortDateString()),
                                     Filter = "Xml file (.xml)|*.xml"
                                 };
            //saveWindow.FileName = "pamResult.xml";

            if (saveWindow.ShowDialog() != true) return;
            var exporter = new DataExporter(_monitor.Data);
            exporter.SaveToXml(saveWindow.OpenFile());
        }

        private void OnMenuItemCheckNewVersionClick(object sender, RoutedEventArgs e)
        {
            CheckForNewVersion(true);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            appsTree.Applications.SortDescriptions.Clear();
            appsTree.Applications.SortDescriptions.Add(new SortDescription("TotalUsageTime",ListSortDirection.Ascending));
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            appsTree.Applications.SortDescriptions.Clear();
            appsTree.Applications.SortDescriptions.Add(new SortDescription("TotalUsageTime", ListSortDirection.Descending));
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            appsTree.Applications.Filter = CustomFilter;
            appsTree.Applications.SortDescriptions.Add(new SortDescription("TotalUsageTime", ListSortDirection.Descending));
        }

        private bool CustomFilter(object item)
        {
            var application = item as PAM.Core.Implementation.ApplicationImp.Application;
            return application.TotalTimeInMunites > 1;
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
             appsTree.Applications.Refresh();
        }
    }
}