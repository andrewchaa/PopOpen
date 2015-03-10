using System.Diagnostics;

namespace PopOpen
{
    public interface IFindProcess
    {
        Process Find(string path);
    }
}