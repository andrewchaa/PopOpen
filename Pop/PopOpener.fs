namespace Pop

open System
open System.Diagnostics
open System.Threading
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

    [<DllImport("user32.dll")>]
    extern bool GetWindowRect(IntPtr hWnd, RECT& lpRect)

module PopOpen =
    let Open (filePath: string) = 
        let openingProcess = Process.Start filePath
        Thread.Sleep(7000)
        
        let filePaths = [ filePath ]
        let openingProcess = 
            filePaths
            |> List.toSeq
            |> Pop.Cs.InUseDetection.GetProcessesUsingFiles
            |> List.ofSeq<Process>
            |> Seq.head

        let HWND_TOPMOST = new IntPtr -1
        let HWND_NOTOPMOST = new IntPtr -2;
        let SWP_SHOWWINDOW = 0x0040u
        
        let mutable rect = new InteropNative.RECT()
        
        let getWindowRect = InteropNative.GetWindowRect(openingProcess.MainWindowHandle, &rect)
        printfn "rect: %d %d %d %d" rect.Top rect.Left rect.Right rect.Bottom

        let x, y, cx, cy = rect.Left, rect.Top, (rect.Right - rect.Left), (rect.Bottom - rect.Top)
        let topWindow = InteropNative.SetWindowPos (openingProcess.MainWindowHandle, HWND_TOPMOST, x, y, cx, cy, SWP_SHOWWINDOW)
        let noTopWIndow = InteropNative.SetWindowPos (openingProcess.MainWindowHandle, HWND_NOTOPMOST, x, y, cx, cy, SWP_SHOWWINDOW)

        printfn "process: %d" openingProcess.Id
        openingProcess.Id
        
            
            

