using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PopOpen
{
    public class ProcessFinder : IFindProcess
    {
        public Process Find(string path)
        {
            return InUseDetection.GetProcessesUsingFiles(new List<string> {path}).First();
        }
    }
}