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
    let SelectHandle i j (k: Input) log  = 
        let r1 = i k
        let r2 = j k
        match r1 with
        | 0n -> r2
        | _ -> r1


//    [<Test>]
//    member x. ``It should start the process`` () = 
//        let FakeStart _ = { File = "Start called"; Prc = new Process() }
//
//        OpenInternal FakeStart 1 FakeFindLockHandle FakeFindProcHandle SelectHandle Log "test.txt" |> should equal "Start called"

//    [<Test>]
//    member x. ``It should find the process handle`` () =
//        let FakeFindProcHandle _ = "Proc handle found"
//
//        OpenInternal FakeStart FakeFindProcHandle FakeFindLockHandle SelectHandle "test.txt" |> should equal "Proc handle found"
//
//    [<Test>]
//    member x. ``It should check the locking process's handle`` () = 
//        let FakeFindLockHandle _ = "Lock handle found"
//        let SelectHandle i j k = j k 
//
//        OpenInternal FakeStart FakeFindProcHandle FakeFindLockHandle SelectHandle "test.txt" |> should equal "Lock handle found"

        
    [<Test>] 
    member x. ``When find the locking handle of a file, it uses it`` ()=
        let start f = { File = f; Prc = new Process() }
        let FakeFindProcHandle f = nativeint 0
        let FakeFindLockHandle p = nativeint 10

        OpenInternal start 1 FakeFindLockHandle FakeFindProcHandle SelectHandle Log "file" |> should equal (nativeint 10)

    [<Test>] 
    member x. ``If you can't find the lock handle, use the proc handle`` ()=
        let start f = { File = f; Prc = new Process() }
        let FakeFindLockHandle _  = 0n
        let FakeFindProcHandle _  = 20n

        OpenInternal start 1 FakeFindLockHandle FakeFindProcHandle SelectHandle Log "file" |> should equal (nativeint 20)


