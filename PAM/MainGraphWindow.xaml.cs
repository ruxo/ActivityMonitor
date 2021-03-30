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
        WindowState lastWindowState;
        bool        shouldClose;
        AppMonitor  monitor = null!;

        public MainGraphWindow()
        {
            InitializeComponent();

            Core.SettingsManager.Settings.SettingsProvider = new SettingsProvider();

            Task.Run(() => CheckForNewVersion());
        }

        void CheckForNewVersion(bool showMessageWhenNoNewVersion = false)
        {
            var version        = VersionChecker.GetLatestVersionInfo();
            var currentVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;
            if (version != null && string.Compare(version.Version, currentVersion, StringComparison.Ordinal) > 0)
                Popup(new NewVersionPopupView {DataContext = version});
            else if (showMessageWhenNoNewVersion) Popup(new NoNewVersionPopupView());
        }

        protected override void OnStateChanged(EventArgs e)
        {
            lastWindowState = WindowState;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (shouldClose) return;
            e.Cancel = true;
            Hide();
        }

        void OnNotificationAreaIconDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) Open();
        }

        void OnMenuItemOpenClick(object sender, EventArgs e)
        {
            Open();
        }

        void Open()
        {
            Show();
            WindowState = lastWindowState;
        }

        void OnMenuItemExitClick(object sender, EventArgs e)
        {
            shouldClose = true;
            Close();
        }

        void FormLoaded(object sender, RoutedEventArgs e)
        {
            monitor                 =  new(Dispatcher);
            appsTree.Applications   =  (CollectionView) monitor.SortedData;
            CurrentApp.DataContext  =  monitor;
            monitor.PropertyChanged += _monitor_PropertyChanged;

            new SettingsProvider().RunAutoExport(monitor);
        }

        void _monitor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.InvokeIfRequired(() => appsTree.Applications.Refresh());
        }

        void OnMenuItemSettingsClick(object sender, EventArgs e)
        {
            new Settings().ShowDialog();
        }

        void OnMenuItemAboutClick(object sender, EventArgs e)
        {
            new AboutBox(null).Show();
        }

        void OnMenuItemExportClick(object sender, EventArgs e)
        {
            var saveWindow = new SaveFileDialog
                                 {
                                     FileName = $"Personal Activity Monitor ({DateTime.Now.ToShortDateString()}).xml",
                                     Filter   = "Xml file (.xml)|*.xml"
                                 };
            //saveWindow.FileName = "pamResult.xml";

            if (saveWindow.ShowDialog() != true) return;
            var exporter = new DataExporter(monitor.Data);
            exporter.SaveToXml(saveWindow.OpenFile());
        }

        void OnMenuItemCheckNewVersionClick(object sender, RoutedEventArgs e)
        {
            CheckForNewVersion(true);
        }

        void button1_Click(object sender, RoutedEventArgs e)
        {
            appsTree.Applications.SortDescriptions.Clear();
            appsTree.Applications.SortDescriptions.Add(new SortDescription("TotalUsageTime", ListSortDirection.Ascending));
        }

        void button2_Click(object sender, RoutedEventArgs e)
        {
            appsTree.Applications.SortDescriptions.Clear();
            appsTree.Applications.SortDescriptions.Add(new SortDescription("TotalUsageTime", ListSortDirection.Descending));
        }

        bool CustomFilter(object item)
        {
            var application = item as PAM.Core.Implementation.ApplicationImp.Application;
            return application.TotalTimeInMunites > 1;
        }

        void Popup(UIElement view) => taskbarIcon.ShowCustomBalloon(view, PopupAnimation.Slide, 5000);
    }
}