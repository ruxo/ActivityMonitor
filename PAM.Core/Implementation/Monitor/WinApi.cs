using System;
using System.Runtime.InteropServices;
using LanguageExt;
using static LanguageExt.Prelude;

namespace PAM.Core.Implementation.Monitor
{
    public static class WinApi
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);

        [DllImport("User32.dll")]
        static extern bool GetLastInputInfo(ref LastInputInfo plii);

        public static Option<LastInputInfo> GetLastInputInfo()
        {
            var inputInfo = new LastInputInfo();
            inputInfo.cbSize = (uint)Marshal.SizeOf(inputInfo);
            return GetLastInputInfo(ref inputInfo)? Some(inputInfo) : None;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LastInputInfo
        {
            public uint cbSize;
            public uint dwTime;
        }
    }
}