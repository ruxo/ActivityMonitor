using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using LanguageExt;
using PAM.Core.Abstract;
using PAM.Core.Implementation.ApplicationImp;
using RZ.Foundation.Extensions;
using static LanguageExt.Prelude;

namespace PAM.Core.Implementation.Monitor
{
    public sealed class AppUpdater
    {
        static   Applications? _applications;
        readonly Dispatcher    dispatcher;
        string                 previousApplicationName = string.Empty;

        public static double GetMaxValue => _applications?.Select(app => app.TotalUsageTime.TotalSeconds).Max() ?? 0;

        public AppUpdater(Applications applications, Dispatcher dispatcher)
        {
            _applications   = applications;
            this.dispatcher = dispatcher;
        }
    }
}