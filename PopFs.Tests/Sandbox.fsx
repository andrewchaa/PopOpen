let testLoop = async {
    for i in [1..100] do
        // do something
        printf "%i before.." i
        
        // sleep a bit 
        do! Async.Sleep 10  
        printfn "..after"
    }

Async.RunSynchronously testLoop


open System
open System.Threading

// create a cancellation source
let cancellationSource = new CancellationTokenSource()

// start the task, but this time pass in a cancellation token
Async.Start (testLoop,cancellationSource.Token)

// wait a bit
Thread.Sleep(200)  

// cancel after 200ms
cancellationSource.Cancel()


let fn1 = async {
        do! Async.Sleep 1000
        printfn "fn1 finished!"
        return 5
    }

let fn2 = async {
        do! Async.Sleep 1500
        printfn "fn2 finished!"
        return "a string"
    }

let fncombined = async {
        // start both computations simultaneously
        let! fn1 = Async.StartChild fn1
        let! fn2 = Async.StartChild fn2

        // retrieve results
        let! result1 = fn1
        let! result2 = fn2

        return sprintf "%d, %s" (result1 + 5) (result2.ToUpper())
    }

fncombined
|> Async.RunSynchronously
|> printfn "%A"
