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
        if prc = null then nativeint 0
        else prc.MainWindowHandle


    let internal GetLockHandle (file: string) =
        file
        |> fun f -> Cs.InUseDetection.GetProcessesUsingFiles [f]
        |> List.ofSeq<Process>
        |> fun p -> if p.Length = 0 then nativeint 0 else p.Head.MainWindowHandle


    let internal SelectHandle (x: nativeint) (y: nativeint) = 
        if x <> nativeint 0 then x
        else y                            

    let internal WaitingFor handle timeToStop =
        handle = nativeint 0 && DateTime.Now < timeToStop

    let internal Find getLockHandle getProcessHandle (waitSeconds: int) ((file: string), (p: Process)) = 

        let mutable handle = nativeint 0
        let timeToStop = DateTime.Now.AddSeconds(float waitSeconds)

        while WaitingFor handle timeToStop  do
            Thread.Sleep 1000

            handle <- SelectHandle (getLockHandle file) (getProcessHandle p)
            Debug.WriteLine ("Handle: {0}", handle)

        handle

    let internal PopUp handle =
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

        handle


    let OpenInternal start file (waitTime: int) getLockHandle getProcessHandle = 

        file 
        |> start 
        |> Find getLockHandle getProcessHandle waitTime
        |> PopUp


    let Open (file: string) = 
        OpenInternal Start file 8 GetLockHandle GetProcessHandle

    let OpenW (file: string, waitSeconds) = 
        OpenInternal Start file waitSeconds GetLockHandle GetProcessHandle


type Launcher () = 
    interface IPopOpen with
        member x.Open (file: string) = 
            PopOpen.OpenInternal PopOpen.Start file 8 PopOpen.GetLockHandle PopOpen.GetProcessHandle
    
        member x.Open (file: string, waitSeconds: int) =
            PopOpen.OpenInternal PopOpen.Start file waitSeconds PopOpen.GetLockHandle PopOpen.GetProcessHandle
    
           
            

