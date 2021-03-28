using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace PAM.Tests.SomeGeneralTestings
{

    [TestFixture]
    public class ReadingProcessesTests
    {
        

        [Test, Ignore("Unfinished?")]
        public void ReadingProcesses()
        {
            for (var i = 0; i > -1; i++)
            {
                const int chars = 256;
                var buff = new StringBuilder(chars);

                // Obtain the handle of the active window.
                var handle = GetForegroundWindow();

                // Update the controls.
                if (GetWindowText(handle, buff, chars) <= 0) continue;
                //Debug.WriteLine(buff.ToString());
                //Debug.WriteLine(handle.ToString());

                int processId;
                GetWindowThreadProcessId(new HandleRef(null,handle) , out processId);

                try {
                    var process = Process.GetProcessById(processId);
                    //Debug.WriteLine(process.ProcessName);
                    Debug.WriteLine(process.MainModule.FileVersionInfo.FileDescription + " [" + process.MainModule.FileVersionInfo.FileName+"]");
                }
                catch (Exception ex) {

                    Debug.WriteLine(ex.Message);
                }

                

                Thread.Sleep(new TimeSpan(0, 0, 0, 1));
            }

        }


        // Declare external functions.
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);
    }
}