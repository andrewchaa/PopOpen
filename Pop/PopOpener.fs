namespace Pop

open System
open System.Diagnostics
open System.Threading

module PopOpen =
    let Open (filePath: string) = 

        let HWND_TOPMOST = new IntPtr -1
        let HWND_NOTOPMOST = new IntPtr -2;
        let SWP_SHOWWINDOW = 0x0040u

        let openingProcess = Process.Start filePath
        let noHandle = filePath |> ProcessFinder.Find 
        printf "handle, immediately: %d\n" noHandle

        Thread.Sleep(7000)
        
        let handle =
            filePath
            |> ProcessFinder.Find
            |> Peekaboo.GetWindowPositions
            |> Peekaboo.SetWindowPosition

        printfn "handle: %d\n" handle

        handle
        
            
            

