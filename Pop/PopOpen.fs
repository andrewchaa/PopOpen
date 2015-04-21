namespace Pop

open System
open System.Diagnostics
open System.Threading
open System.IO
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
        |> fun f -> [f]
        |> InUseDetection.GetProcessesUsingFiles
        |> List.ofSeq<Process>
        |> fun ps -> match ps with
                     | [] -> nativeint 0
                     | p :: _ -> p.MainWindowHandle


    let internal SelectHandle (x: nativeint) (y: nativeint) log = 
        log (String.Format("Lock Handle: {0}, Proc Handle: {1}", x, y))
        if x <> nativeint 0 then x
        else y                            


    let BringToFront handle log =
        let HWND_TOPMOST = new IntPtr -1
        let HWND_NOTOPMOST = new IntPtr -2;
        let SWP_SHOWWINDOW = 0x0040u

        let mutable rect = new InteropNative.RECT()
        let result = InteropNative.GetWindowRect(handle, &rect)

        log (String.Format("Popping up {0}, Rect: {1} {2} {3} {4}", handle, rect.Top, rect.Left, rect.Right, rect.Bottom))
        
        rect.Width <- rect.Right - rect.Left
        rect.Height <- rect.Bottom - rect.Top

        InteropNative.SetWindowPos (handle, HWND_TOPMOST, rect.Left, rect.Top, rect.Width, rect.Height, SWP_SHOWWINDOW) |> ignore
        InteropNative.SetWindowPos (handle, HWND_NOTOPMOST, rect.Left, rect.Top, rect.Width, rect.Height, SWP_SHOWWINDOW) |> ignore
        ()


    let PopUp getLockHandle getProcessHandle (waitTime: float) log ((file: string), (p: Process)) =
        let timeToStop = DateTime.UtcNow.AddSeconds(waitTime)

        

        let rec popUpLoop oldHandle currentTime =
            Thread.Sleep 500
            let newHandle = SelectHandle (getLockHandle file) (getProcessHandle p) log
            log ("Old: " + oldHandle.ToString() + " New: " + newHandle.ToString())

            if (oldHandle <> newHandle) then BringToFront newHandle log
            Thread.Sleep 1000
            if (oldHandle <> newHandle) then BringToFront newHandle log
            
            if currentTime > timeToStop
            then newHandle
            else popUpLoop newHandle DateTime.UtcNow

        popUpLoop (nativeint 0) DateTime.UtcNow            
            

    let OpenInternal start file (waitTime: int) getLockHandle getProcessHandle log = 

        file 
        |> start 
        |> PopUp getLockHandle getProcessHandle (float waitTime) log


    let WaitSeconds = 30

    let Open (file: string) = 
        OpenInternal Start file WaitSeconds GetLockHandle GetProcessHandle (fun f -> Debug.WriteLine f)

    let OpenW (file: string, waitSeconds) = 
        OpenInternal Start file waitSeconds GetLockHandle GetProcessHandle (fun f -> Debug.WriteLine f)


    let logToFile (l: string) =
        let path = Path.Combine(Path.GetTempPath(), "pop.log")
        
        use writer = new StreamWriter(path, true)
        l |> writer.WriteLine |> ignore

        ()

    let OpenD (file: string) = 
        OpenInternal Start file WaitSeconds GetLockHandle GetProcessHandle (fun f ->  logToFile f)
