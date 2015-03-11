using System;

namespace Pop.Contracts
{
    public interface IPeekaboo
    {
        IntPtr Minimise(IntPtr handle);
        IntPtr Restore(IntPtr handle);
    }
}