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

        let mutable handle = nativeint 0
        let mutable counter = 0

        while handle = nativeint 0 && counter < 10  do
            handle <- ProcessFinder.Find filePath
            Thread.Sleep(1000)
            printf "counter: %d\n" counter
            counter <- counter + 1

        handle <-
            filePath
            |> ProcessFinder.Find
            |> Peekaboo.GetWindowPositions
            |> Peekaboo.SetWindowPosition

        printfn "handle: %d\n" handle

        handle
        
            
            

