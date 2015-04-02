using System;

namespace Pop.Cs
{
    public interface IPopOpen
    {
        IntPtr Open(string file);
        IntPtr Open(string file, int waitSeconds);
    }
}