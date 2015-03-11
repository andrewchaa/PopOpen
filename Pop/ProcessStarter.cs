using System.Diagnostics;
using Pop.Contracts;

namespace Pop
{
    public class ProcessStarter : IStartProcess
    {
        public void Start(string filePath)
        {
            Process.Start(filePath);
        }
    }
}