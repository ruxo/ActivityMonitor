using System;
using System.Runtime.InteropServices;

namespace PAM.Core.Implementation.Monitor
{
    public static class WinApi
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);

        [DllImport("User32.dll")]
        public static extern bool GetLastInputInfo(ref Lastinputinfo plii);

        [StructLayout(LayoutKind.Sequential)]
        public struct Lastinputinfo
        {
            public uint cbSize;
            public uint dwTime;
        }
    }
}