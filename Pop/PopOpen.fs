namespace Pop

open System
open System.Diagnostics
open System.Threading
open System.IO
open Pop.Cs

module PopOpen =

    type Result<'TEntity> =
        | Success of 'TEntity
        | Failure of string

    
    type Input = { File: string; Prc: Process }

    let internal Start (file: string) = 
        {
            File = file;
            Prc = file |> Process.Start
        }

    let internal FindProcessHandle (input : Input) =
        try
            Success input.Prc.MainWindowHandle
        with
            | :? System.InvalidOperationException -> Failure "InvalidOperationException"
            | :? System.NullReferenceException -> Failure "NullReferenceException"


    let internal FindLockHandle (input: Input) =
        
        input.File
        |> fun f -> [f]
        |> InUseDetection.GetProcessesUsingFiles
        |> List.ofSeq<Process>
        |> fun ps -> match ps with
                     | [] -> Failure "Can't find the process"
                     | p :: _ -> Success p.MainWindowHandle


    let SelectHandle findProcHandle findLockHandle (input: Input) log = 
        let procHandle = findProcHandle input
        let lockHandle = findLockHandle input
        
        log (String.Format("Lock Handle: {0}, Proc Handle: {1}", lockHandle, procHandle))

        match lockHandle with
        | Success h -> h
        | Failure s -> match procHandle with
                       | Success h -> h
                       | Failure s -> 0n


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
        Thread.Sleep 300
        InteropNative.SetWindowPos (handle, HWND_TOPMOST, rect.Left, rect.Top, rect.Width, rect.Height, SWP_SHOWWINDOW) |> ignore
        InteropNative.SetWindowPos (handle, HWND_NOTOPMOST, rect.Left, rect.Top, rect.Width, rect.Height, SWP_SHOWWINDOW) |> ignore
        ()


    let PopUp findLockHandle findProcessHandle selectHandle (waitTime: float) log (input: Input) =
        let timeToStop = DateTime.UtcNow.AddSeconds(waitTime)

        let rec popUpLoop oldHandle currentTime =
            Thread.Sleep 500

            let newHandle = selectHandle findLockHandle findProcessHandle input log
            if (oldHandle <> newHandle) then BringToFront newHandle log
            log ("Old: " + oldHandle.ToString() + " New: " + newHandle.ToString())

            if currentTime > timeToStop 
            then newHandle
            else popUpLoop newHandle DateTime.UtcNow

        popUpLoop (nativeint 0) DateTime.UtcNow            
            

    let OpenInternal start (waitTime: int) findLockHandle findProcessHandle selectHandle log file = 

        file 
        |> start 
        |> PopUp findLockHandle findProcessHandle selectHandle (float waitTime) log


    let WaitSeconds = 30

    let Open (file: string) = 
        OpenInternal Start WaitSeconds FindLockHandle FindProcessHandle SelectHandle (fun f -> Debug.WriteLine f) file 

    let OpenW (file: string, waitSeconds) = 
        OpenInternal Start waitSeconds FindLockHandle FindProcessHandle SelectHandle (fun f -> Debug.WriteLine f) file

    let logToFile (l: string) =
        let path = Path.Combine(Path.GetTempPath(), "pop.log")
        
        use writer = new StreamWriter(path, true)
        l |> writer.WriteLine |> ignore

        ()

    let OpenD (file: string) = 
        OpenInternal Start WaitSeconds FindLockHandle FindProcessHandle SelectHandle (fun f ->  logToFile f) file
