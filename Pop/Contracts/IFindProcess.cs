using System;

namespace Pop.Contracts
{
    public interface IFindProcess
    {
        PopProcess Find(string path);
        IntPtr FindForegroundWindow();
    }
}