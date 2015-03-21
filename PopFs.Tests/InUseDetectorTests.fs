namespace PopFs.Tests

open Pop.Cs
open Pop
open NUnit.Framework
open FsUnit
open System
open System.IO
open System.Threading

[<TestFixture>]
type ``Given InUseDetection`` ()=
    let openFile = 
        let file = Path.Combine(Environment.CurrentDirectory, "IAmTextFile.txt")
        System.Diagnostics.Process.Start (file) |> ignore
        Thread.Sleep(2000)
        InUseDetection.GetProcessesUsingFiles [file]

    [<Test>] member t.
        ``When it opens a text file`` ()=
            openFile |> should haveCount 1