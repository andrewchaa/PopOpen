namespace PopFs.Tests

open NUnit.Framework
open FsUnit
open System.Diagnostics
open Pop
open PopOpen

[<TestFixture>]
type ``Given OpenInternal`` ()=
    
    let FakeStart i = i
    let FakeFindProcHandle _ = nativeint 0
    let FakeFindLockHandle _ = nativeint 0
    let SelectHandle i j k = if i > j then i else j
    let Log f = ()


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
        let getLockHandle f = nativeint 10
        let getProcessHandle p = nativeint 0

        PopOpen.OpenInternal start 1 getLockHandle getProcessHandle SelectHandle Log "file" |> should equal (nativeint 10)

    [<Test>] 
    member x. ``When can't find the locking handle of a file, it uses the handle from Process.Start`` ()=
        let start f = { File = f; Prc = new Process() }
        let getLockHandle f = nativeint 0
        let getProcessHandle p = nativeint 10
        let Log f = ()

        PopOpen.OpenInternal start 1 getLockHandle getProcessHandle SelectHandle Log "file" |> should equal (nativeint 10)


