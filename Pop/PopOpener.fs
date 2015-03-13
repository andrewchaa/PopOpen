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
        
        let topWindow = InteropNative.SetWindowPos (openingProcess.MainWindowHandle, HWND_TOPMOST, 0, 0, 800, 600, SWP_SHOWWINDOW)
        let noTopWIndow = InteropNative.SetWindowPos (openingProcess.MainWindowHandle, HWND_NOTOPMOST, 0, 0, 800, 600, SWP_SHOWWINDOW)

        openingProcess.Id
        // printfn "process: %d" openingProcess.Id
            
            

