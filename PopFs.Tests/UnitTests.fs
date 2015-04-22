﻿namespace PopFs.Tests

open NUnit.Framework
open FsUnit
open Pop
open System.Diagnostics

[<TestFixture>]
type ``Given OpenInternal`` ()=
    
        
    [<Test>] 
    member x. ``When find the locking handle of a file, it uses it`` ()=
        let start f = f, new Process()
        let getLockHandle f = nativeint 10
        let getProcessHandle p = nativeint 0
        let log f = ()

        PopOpen.OpenInternal start "file" 1 getLockHandle getProcessHandle log |> should equal (nativeint 10)

    [<Test>] 
    member x. ``When can't find the locking handle of a file, it uses the handle from Process.Start`` ()=
        let start f = f, new Process()
        let getLockHandle f = nativeint 0
        let getProcessHandle p = nativeint 10
        let log f = ()

        PopOpen.OpenInternal start "file" 1 getLockHandle getProcessHandle log |> should equal (nativeint 10)

