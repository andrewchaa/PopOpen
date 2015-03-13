namespace Pop

open System.Diagnostics

module ProcessFinder =
    let Find (filePath: string) =
        let filePaths = [ filePath ]

        let processes = 
            filePaths
            |> List.toSeq
            |> Cs.InUseDetection.GetProcessesUsingFiles
            |> List.ofSeq<Process>

        match processes.Length with
        | 0 -> nativeint 0
        | _ -> processes.Head.MainWindowHandle

        
                



