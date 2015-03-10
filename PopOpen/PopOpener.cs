using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace PopOpen
{
    public class PopOpener
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        const int WM_SYSCOMMAND = 274;
        const int SC_MINIMIZE = 0xF020;
        const int SC_RESTORE = 0xF120;

        private readonly IStartProcess _processStarter;
        private readonly IFindProcess _processFinder;

        public PopOpener(IStartProcess processStarter, IFindProcess processFinder)
        {
            _processStarter = processStarter;
            _processFinder = processFinder;
        }

        public Process Open(string filePath)
        {
            _processStarter.Start(filePath);
            
            Thread.Sleep(7000);
            
            var process = _processFinder.Find(filePath);
            SendMessage(process.MainWindowHandle, WM_SYSCOMMAND, SC_MINIMIZE, 0);
            SendMessage(process.MainWindowHandle, WM_SYSCOMMAND, SC_RESTORE, 0);

            return process;
        }
    }
}
