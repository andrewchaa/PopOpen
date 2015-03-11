using System;

namespace Pop.Cs
{
    public class PopProcess
    {
        public IntPtr MainWindowHandle { get; private set; }
        public int Id { get; private set; }
        public string ProcessName { get; private set; }
        public IntPtr Handle { get; set; }

        public PopProcess(int id, string processName, IntPtr mainWindowHandle)
        {
            Id = id;
            ProcessName = processName;
            MainWindowHandle = mainWindowHandle;
        }
    }
}