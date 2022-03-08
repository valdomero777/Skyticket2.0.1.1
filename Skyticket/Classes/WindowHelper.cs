using System;
using System.Runtime.InteropServices;

namespace Skyticket
{
    class WindowHelper
    {
        [DllImport("User32.dll")]
        internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        internal static readonly IntPtr InvalidHandleValue = IntPtr.Zero;
        internal const int SW_NORMAL = 1;

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr SetFocus(HandleRef hWnd);


        [DllImport("user32.dll", SetLastError = true)]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        public static void ActivateEx(IntPtr WndHandle)
        {
            //Process currentProcess = Process.GetCurrentProcess();
            if (WndHandle != InvalidHandleValue)
            {
                SetForegroundWindow(WndHandle);
            }
        }
    }
}
