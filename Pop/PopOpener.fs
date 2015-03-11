namespace Pop

open System.Diagnostics

type PopOpen () = 

    member x.Open(filePath: string) = 
        Process.Start filePath
