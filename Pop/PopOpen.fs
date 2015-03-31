namespace Pop

open System
open System.Diagnostics
open System.Threading

module PopOpen =

    type PopProcess() =
        
        let mutable oProcess = new Process()
        
        member p.Start (file: string) = file |> Process.Start |> fun p -> oProcess <- p
        member p.Handle = oProcess.MainWindowHandle

//    let internal Start (file: string) =
//        file |> Process.Start

    

    let GetLockingProcessHandle (file: string) =
        file
        |> fun f -> Cs.InUseDetection.GetProcessesUsingFiles [f]
        |> List.ofSeq<Process>
        |> fun p -> if p.Length = 0 then nativeint 0 else p.Head.MainWindowHandle

    let Find (file: string) func = 

        let mutable handle = nativeint 0
        let mutable counter = 0
            
        while handle = nativeint 0 && counter < 8  do
            Thread.Sleep 1000

            handle <- func file
            Debug.WriteLine ("Counter: {0}, Handle: {1}", counter, handle)
            counter <- counter + 1

        handle


    let internal PopUp (handle: IntPtr) =
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


    let OpenInternal start file handle getLockingProcessHandle = 
        file |> start

        let winHandle = Find file getLockingProcessHandle
        if winHandle <> nativeint 0 
        then winHandle |> PopUp
        else handle |> PopUp


    let Open (file: string) = 
        let popProcess = new PopProcess()

        OpenInternal popProcess.Start file popProcess.Handle GetLockingProcessHandle
        
            
            

