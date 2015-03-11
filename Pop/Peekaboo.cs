using System;
using System.Runtime.InteropServices;
using Pop.Contracts;

namespace Pop
{
    public class Peekaboo : IPeekaboo
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        [DllImport("User32.dll")]
        private static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        private const int WM_SYSCOMMAND = 274;
        private const int SC_MINIMIZE = 0xF020;
        private const int SC_MAXIMIZE = 0xF030;
        private const int SC_RESTORE = 0xF120;

        public IntPtr Minimise(IntPtr handle)
        {
            return SendMessage(handle, WM_SYSCOMMAND, SC_MINIMIZE, 0);
        }

        public IntPtr Maximize(IntPtr handle)
        {
            return SendMessage(handle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
        }

        public IntPtr Restore(IntPtr handle)
        {
            var pointer = SendMessage(handle, WM_SYSCOMMAND, SC_RESTORE, 0);
            SetForegroundWindow(handle);

            return pointer;
        }
    }
}