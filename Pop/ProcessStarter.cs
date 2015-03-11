using System.Diagnostics;
using Pop.Cs.Contracts;

namespace Pop.Cs
{
    public class ProcessStarter : IStartProcess
    {
        public void Start(string filePath)
        {
            Process.Start(filePath);
        }
    }
}