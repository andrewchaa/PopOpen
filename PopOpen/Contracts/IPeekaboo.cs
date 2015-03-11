using System;

namespace PopOpen.Contracts
{
    public interface IPeekaboo
    {
        IntPtr Minimise(IntPtr handle);
        IntPtr Restore(IntPtr handle);
    }
}