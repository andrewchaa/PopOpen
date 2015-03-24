namespace Pop

open System
open System.Diagnostics
open System.Threading

module PopOpen =

    let Start (file: string) =

        (file, file |> Process.Start)

    let Find (file: string, openingProcess: Process) =

        let findHandle (file: string) =
            file
            |> fun f -> Cs.InUseDetection.GetProcessesUsingFiles [f]
            |> List.ofSeq<Process>
            |> fun p -> if p.Length = 0 then nativeint 0 else p.Head.MainWindowHandle

        let mutable handle = nativeint 0
        let mutable counter = 0
            
        while handle = nativeint 0 && counter < 8  do
            Thread.Sleep(1000)
            handle <- findHandle file
            Debug.WriteLine "Counter: {0}, Handle: {1}", counter, handle  //Debug.WriteLine
            counter <- counter + 1

        if handle = nativeint 0 then handle <- openingProcess.MainWindowHandle

        handle


    let GetWindowPositions (handle: IntPtr) = 
        let mutable rect = new InteropNative.RECT()
        let result = InteropNative.GetWindowRect(handle, &rect)

        Debug.WriteLine ("Rect: {0} {1} {2} {3}", rect.Top, rect.Left, rect.Right, rect.Bottom)
        
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

    let OpenX start find getPositions setPositions file = 
        file
        |> start
        |> find
        |> getPositions
        |> setPositions


    let Open (file: string) =
        OpenX Start Find GetWindowPositions SetWindowPositions file
        
            
            

