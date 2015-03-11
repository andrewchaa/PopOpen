using System.Diagnostics;
using PopOpen.Contracts;

namespace PopOpen
{
    public class ProcessStarter : IStartProcess
    {
        public void Start(string filePath)
        {
            Process.Start(filePath);
        }
    }
}