namespace Pop

open System
open System.Diagnostics
open System.Threading

module PopOpen =

    let Start (file: string) =
        let mutable handle = nativeint 0
        let mutable counter = 0

        (file, file |> Process.Start)

    let Find (file: string, openingProcess: Process) =

        let findHandle (file: string) =
            file
            |> fun f -> [f]
            |> List.toSeq
            |> Cs.InUseDetection.GetProcessesUsingFiles
            |> List.ofSeq<Process>
            |> fun p -> if p.Length = 0 then nativeint 0 else p.Head.MainWindowHandle

        let mutable handle = nativeint 0
        let mutable counter = 0
            
        while handle = nativeint 0 && counter < 8  do
            Thread.Sleep(1000)
            handle <- findHandle file
            printf "counter: %d\n" counter
            printf "handle: %d\n" handle
            counter <- counter + 1

        if handle > nativeint 0 
        then handle
        else openingProcess.MainWindowHandle


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

    let Open (file: string) = 

        let handle =
            file 
            |> Start
            |> Find
            |> GetWindowPositions
            |> SetWindowPositions

        printfn "handle: %d\n" handle

        handle
        
            
            

