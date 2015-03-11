using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Pop.Contracts;

namespace Pop
{
    public class ProcessFinder : IFindProcess
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        public PopProcess Find(string path)
        {
            var process = InUseDetection.GetProcessesUsingFiles(new List<string> {path}).First();
            
            return new PopProcess(process.Id, process.ProcessName, process.MainWindowHandle);
        }

        public IntPtr FindForegroundWindow()
        {
            return GetForegroundWindow();
        }
    }
}