namespace Pop

open System
open System.Diagnostics
open System.Threading
open Pop.Cs

module PopOpen =

    type Result<'nativeint> =
        | Success
        | Failure

    let internal Start (file: string) =
        file, file |> Process.Start


    let internal GetProcessHandle (prc: Process) =
        try
            prc.MainWindowHandle
        with
            | :? System.InvalidOperationException -> nativeint 0
            | :? System.NullReferenceException -> nativeint 0


    let internal GetLockHandle (file: string) =
        file
        |> fun f -> Cs.InUseDetection.GetProcessesUsingFiles [f]
        |> List.ofSeq<Process>
        |> fun p -> if p.Length = 0 then nativeint 0 else p.Head.MainWindowHandle


    let internal SelectHandle (x: nativeint) (y: nativeint) = 
        Debug.WriteLine ("Lock Handle: {0}, Proc Handle: {1}", x, y)
        if x <> nativeint 0 then x
        else y                            


    let BringToFront handle =
        let HWND_TOPMOST = new IntPtr -1
        let HWND_NOTOPMOST = new IntPtr -2;
        let SWP_SHOWWINDOW = 0x0040u

        let mutable rect = new InteropNative.RECT()
        let result = InteropNative.GetWindowRect(handle, &rect)

        Debug.WriteLine ("Rect: {0} {1} {2} {3}", rect.Top, rect.Left, rect.Right, rect.Bottom)
        
        rect.Width <- rect.Right - rect.Left
        rect.Height <- rect.Bottom - rect.Top

        InteropNative.SetWindowPos (handle, HWND_TOPMOST, rect.Left, rect.Top, rect.Width, rect.Height, SWP_SHOWWINDOW) |> ignore
        InteropNative.SetWindowPos (handle, HWND_NOTOPMOST, rect.Left, rect.Top, rect.Width, rect.Height, SWP_SHOWWINDOW) |> ignore
        ()


    let PopUp getLockHandle getProcessHandle (waitTime: float) ((file: string), (p: Process)) =
        let timeToStop = DateTime.UtcNow.AddSeconds(waitTime)
                
        let rec popUpLoop oldHandle currentTime =
            Thread.Sleep 1000
            let newHandle = SelectHandle (getLockHandle file) (getProcessHandle p)
            Debug.WriteLine ("Old handle: {0}, New Handle: {1}", oldHandle, newHandle)

            if (oldHandle <> newHandle) then BringToFront newHandle
            
            if currentTime > timeToStop
            then newHandle
            else popUpLoop newHandle DateTime.UtcNow

        popUpLoop (nativeint 0) DateTime.UtcNow            
            

    let OpenInternal start file (waitTime: int) getLockHandle getProcessHandle = 

        file 
        |> start 
        |> PopUp getLockHandle getProcessHandle (float waitTime)


    let WaitSeconds = 30
    let Open (file: string) = 
        OpenInternal Start file WaitSeconds GetLockHandle GetProcessHandle

    let OpenW (file: string, waitSeconds) = 
        OpenInternal Start file waitSeconds GetLockHandle GetProcessHandle

