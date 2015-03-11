using System;

namespace PopOpen.Contracts
{
    public interface IFindProcess
    {
        PopProcess Find(string path);
        IntPtr FindForegroundWindow();
    }
}