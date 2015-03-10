using System.Diagnostics;

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