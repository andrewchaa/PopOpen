using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Pop.Contracts;

namespace Pop
{
    public class PopOpener
    {
        [DllImport("user32.dll")]
        static extern bool AllowSetForegroundWindow(int dwProcessId);

        [DllImport("User32.dll")]
        private static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        static extern bool SetWindowPos(
             IntPtr hWnd,           // window handle
             IntPtr hWndInsertAfter,    // placement-order handle
             int X,          // horizontal position
             int Y,          // vertical position
             int cx,         // width
             int cy,         // height
             uint uFlags);       // window positioning flags

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        static readonly IntPtr HWND_TOP = new IntPtr(0);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        private const uint SWP_SHOWWINDOW = 0x0040;

        private readonly IStartProcess _processStarter;
        private readonly IFindProcess _processFinder;
        private readonly IPeekaboo _peekaboo;

        public PopOpener(IStartProcess processStarter, IFindProcess processFinder, IPeekaboo peekaboo)
        {
            _processStarter = processStarter;
            _processFinder = processFinder;
            _peekaboo = peekaboo;
        }

        public PopProcess Open(string filePath)
        {
            var currentProcess = Process.GetCurrentProcess();

            _processStarter.Start(filePath);
            
            Thread.Sleep(7000);

            var process = _processFinder.Find(filePath);
            var foregroundWindowHandle = _processFinder.FindForegroundWindow();

            SetForegroundWindow(currentProcess.MainWindowHandle);
            AllowSetForegroundWindow(currentProcess.Id);

//            if (process.MainWindowHandle != foregroundWindowHandle)
//            {
            _peekaboo.Minimise(process.MainWindowHandle);
            _peekaboo.Restore(process.MainWindowHandle);
            SetWindowPos(process.MainWindowHandle, HWND_TOPMOST, 0, 0, 100, 100, SWP_SHOWWINDOW);

//            }
            

            return process;
        }
    }
}
