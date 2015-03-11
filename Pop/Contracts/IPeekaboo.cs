using System;

namespace Pop.Contracts
{
    public interface IPeekaboo
    {
        IntPtr Minimise(IntPtr handle);
        IntPtr Maximize(IntPtr handle);
        IntPtr Restore(IntPtr handle);
    }
}