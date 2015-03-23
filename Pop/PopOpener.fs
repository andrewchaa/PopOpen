namespace Pop

open System
open System.Diagnostics
open System.Threading

module PopOpen =

    let Start (file: string) =
        let mutable handle = nativeint 0
        let mutable counter = 0

        let openingProcess = file |> Process.Start

        while handle = nativeint 0 && counter < 7  do
            handle <- openingProcess.MainWindowHandle
            Thread.Sleep(1000)
            printf "counter: %d\n" counter
            printf "handle: %d\n" handle
            counter <- counter + 1

        handle

    let Find (filePath: string) =
        let filePaths = [ filePath ]

        let processes = 
            filePaths
            |> List.toSeq
            |> Cs.InUseDetection.GetProcessesUsingFiles
            |> List.ofSeq<Process>

        printfn "processes: %d" processes.Length
        match processes.Length with
        | 0 -> nativeint 0
        | _ -> processes.Head.MainWindowHandle

    let GetWindowPositions (handle: IntPtr) = 
        let mutable rect = new InteropNative.RECT()
        let result = InteropNative.GetWindowRect(handle, &rect)

        printfn "rect: %d %d %d %d" rect.Top rect.Left rect.Right rect.Bottom
        
        rect.Width <- rect.Right - rect.Left
        rect.Height <- rect.Bottom - rect.Top

        (handle, rect)

    let SetWindowPositions (handle: IntPtr, rect: InteropNative.RECT) =

        let HWND_TOPMOST = new IntPtr -1
        let HWND_NOTOPMOST = new IntPtr -2;
        let SWP_SHOWWINDOW = 0x0040u

        let topWindowResult = InteropNative.SetWindowPos (handle, HWND_TOPMOST, rect.Left, rect.Top, rect.Width, rect.Height, SWP_SHOWWINDOW)
        let noTopWindowResult = InteropNative.SetWindowPos (handle, HWND_NOTOPMOST, rect.Left, rect.Top, rect.Width, rect.Height, SWP_SHOWWINDOW)

        handle

    let OpenInt start (file: string) =
        file 
        |> start
        |> fun handle -> if handle > nativeint 0 then handle else Find file
        |> GetWindowPositions
        |> SetWindowPositions
        

    let Open (file: string) = 

        let handle =
            file 
            |> Start
            |> fun handle -> if handle > nativeint 0 then handle else Find file
            |> GetWindowPositions
            |> SetWindowPositions

//        let openingProcess = Start filePath
//
//        let mutable handle = nativeint 0
//        let mutable counter = 0
//
//        while handle = nativeint 0 && counter < 10  do
//            handle <- Find filePath
//            Thread.Sleep(1000)
//            printf "counter: %d\n" counter
//            counter <- counter + 1
//
//        handle <-
//            filePath
//            |> Find
//            |> GetWindowPositions
//            |> SetWindowPositions
//
        printfn "handle: %d\n" handle

        handle
        
            
            

