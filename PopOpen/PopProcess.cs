using System;
using System.Diagnostics;

namespace PopOpen
{
    public class PopProcess
    {
        public IntPtr MainWindowHandle { get; private set; }
        public int Id { get; private set; }
        public string ProcessName { get; private set; }

        public PopProcess(int id, string processName, IntPtr mainWindowHandle)
        {
            Id = id;
            ProcessName = processName;
            MainWindowHandle = mainWindowHandle;
        }
    }
}