namespace Pop

open System.Diagnostics
open System

module Peekaboo = 

    let GetWindowPositions (handle: IntPtr) = 
        let mutable rect = new InteropNative.RECT()
        let result = InteropNative.GetWindowRect(handle, &rect)

        printfn "rect: %d %d %d %d" rect.Top rect.Left rect.Right rect.Bottom
        
        rect.Width <- rect.Right - rect.Left
        rect.Height <- rect.Bottom - rect.Top

        (handle, rect)

    let SetWindowPosition (handle: IntPtr, rect: InteropNative.RECT) =

        let HWND_TOPMOST = new IntPtr -1
        let HWND_NOTOPMOST = new IntPtr -2;
        let SWP_SHOWWINDOW = 0x0040u

        let topWindowResult = InteropNative.SetWindowPos (handle, HWND_TOPMOST, rect.Left, rect.Top, rect.Width, rect.Height, SWP_SHOWWINDOW)
        let noTopWindowResult = InteropNative.SetWindowPos (handle, HWND_NOTOPMOST, rect.Left, rect.Top, rect.Width, rect.Height, SWP_SHOWWINDOW)

        handle

