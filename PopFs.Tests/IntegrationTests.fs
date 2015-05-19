namespace PopFs.Tests

open NUnit.Framework
open FsUnit
open Pop
open System
open System.Diagnostics
open System.IO

[<TestFixture>]
type ``PopOpen Integration tests`` ()=
    
    let dir = Environment.CurrentDirectory

    [<Test>] member t.
        ``When it opens a word file`` ()= 
            PopOpen.OpenW ((Path.Combine(dir, "Word 1.docx")), 10) |> should not' (equal 0)

    [<Test>] member t.
        ``It should open two word files in sequence successfully`` ()=
            PopOpen.OpenW ((Path.Combine(dir, "Word 1.docx")), 30) |> should not' (equal 0);
            PopOpen.OpenW ((Path.Combine(dir, "Word 2.docx")), 30) |> should not' (equal 0)
             

    [<Test>] member t.
        ``It should add passed down logger for debugging purpose`` ()= 
            PopOpen.OpenD (Path.Combine(dir, "Word 1.docx"), Action<string> (fun s -> Debug.WriteLine s)) |> should not' (equal 0)

    [<Test>] member t.
        ``When it opens the first excel file`` ()=
            PopOpen.OpenW ((Path.Combine(dir, "Excel 1.xlsx")), 10) |> should not' (equal 0)
            
    [<Test>] member t.
        ``When it opens the second excel file`` ()=
            PopOpen.OpenW ((Path.Combine(dir, "Excel 2.xlsx")), 10) |> should not' (equal 0)

    [<Test>] member t.
        ``When it opens the first powerpoint file`` ()=
            PopOpen.OpenW ((Path.Combine(dir, "Powerpoint 1.pptx")), 10) |> should not' (equal 0)

    [<Test>] member t.
        ``When it opens the second powerpoint file`` ()=
            PopOpen.OpenW ((Path.Combine(dir, "Powerpoint 2.pptx")), 10) |> should not' (equal 0)
    
    [<Test>] member t.
        ``When it opens a text file`` ()=
            PopOpen.OpenW ((Path.Combine(dir, "IAmTextFile.txt")), 10) |> should not' (equal 0)

    [<Test>] member t.
        ``When it opens a picture`` ()=
            PopOpen.OpenW ((Path.Combine(dir, "Picture.jpg")), 10) |> should not' (equal 0)

    [<Test>] member t.
        ``When it opens a pdf`` ()=
            PopOpen.OpenW ((Path.Combine(dir, "pdf 1.pdf")), 10) |> should not' (equal 0)
