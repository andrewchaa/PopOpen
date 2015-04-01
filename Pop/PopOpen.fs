namespace Pop

open System
open System.Diagnostics
open System.Threading

module PopOpen =

    let internal Start (file: string) =
        file, file |> Process.Start


    let internal GetProcessHandle (prc: Process) =
        prc.MainWindowHandle


    let internal GetLockHandle (file: string) =
        file
        |> fun f -> Cs.InUseDetection.GetProcessesUsingFiles [f]
        |> List.ofSeq<Process>
        |> fun p -> if p.Length = 0 then nativeint 0 else p.Head.MainWindowHandle


    let internal Find getLockHandle getProcessHandle ((file: string), (p: Process)) = 

        let mutable handle = nativeint 0
        let mutable counter = 0
            
        while handle = nativeint 0 && counter < 8  do
            Thread.Sleep 1000

            handle <- getLockHandle file
            Debug.WriteLine ("Counter: {0}, Handle: {1}", counter, handle)
            counter <- counter + 1

        if handle = nativeint 0 then getProcessHandle p
        else handle


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


    let OpenInternal start file getLockHandle getProcessHandle = 

        file 
        |> start 
        |> Find getLockHandle getProcessHandle
        |> PopUp


    let Open (file: string) = 
        OpenInternal Start file GetLockHandle GetProcessHandle
        
            
            

