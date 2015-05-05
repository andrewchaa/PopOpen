namespace PopFs.Tests

open NUnit.Framework
open FsUnit
open System.Diagnostics
open Pop
open PopOpen
open System

[<TestFixture>]
type ``Given OpenInternal`` ()=
    
    let FakeStart i = i
    let FakeFindProcHandle _ = nativeint 0
    let FakeFindLockHandle _ = nativeint 0
    let Log (f: string) = Console.WriteLine f  

        
    [<Test>] 
    member x. ``When find the locking handle of a file, it uses it`` ()=
        let start f = { File = f; Prc = new Process() }
        let FakeFindProcHandle f = nativeint 0
        let FakeFindLockHandle p = nativeint 10
        let SelectHandle i j (k: Input) log  = 
            let r1 = i k
            let r2 = j k
            match r1 with
            | 0n -> r2
            | _ -> r1

        OpenInternal start 1 FakeFindLockHandle FakeFindProcHandle SelectHandle Log "file" |> should equal (nativeint 10)

    [<Test>] 
    member x. ``If you can't find the lock handle, use the proc handle`` ()=
        let start f = { File = f; Prc = new Process() }
        let FakeFindLockHandle _  = 0n
        let FakeFindProcHandle _  = 20n
        let SelectHandle i j (k: Input) log  = 
            let r1 = i k
            let r2 = j k
            match r1 with
            | 0n -> r2
            | _ -> r1

        OpenInternal start 1 FakeFindLockHandle FakeFindProcHandle SelectHandle Log "file" |> should equal 20n

    [<Test>]
    member x. ``It should favour file lock handler more than process handle`` () =
        let FakeFindLockHandle _  = Success 10n
        let FakeFindProcHandle _  = Success 20n
        let input = {
            File = "test";
            Prc = new Process()
        }
        
        SelectHandle FakeFindProcHandle FakeFindLockHandle input Log |> should equal 10n


