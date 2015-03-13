namespace Pop

open System
open System.Runtime.InteropServices

module InteropNative = 
    [<DllImport("user32.dll", EntryPoint = "SetWindowPos")>]
    extern bool SetWindowPos(
            IntPtr hWnd,                // window handle
            IntPtr hWndInsertAfter,     // placement-order handle
            int X,                      // horizontal position
            int Y,                      // vertical position
            int cx,                     // width
            int cy,                     // height
            uint32 uFlags);             // window positioning flags

    [<Struct>]
    type RECT =
        val Left: int
        val Top: int
        val Right: int
        val Bottom: int
        val mutable Width: int
        val mutable Height: int

    [<DllImport("user32.dll")>]
    extern bool GetWindowRect(IntPtr hWnd, RECT& lpRect)


