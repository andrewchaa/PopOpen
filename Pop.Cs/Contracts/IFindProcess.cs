using System;

namespace Pop.Cs.Contracts
{
    public interface IFindProcess
    {
        PopProcess Find(string path);
        IntPtr FindForegroundWindow();
    }
}