namespace PopFs.Tests

open NUnit.Framework
open FsUnit
open Pop
open System
open System.IO

[<TestFixture>]
type ``Given PopOpener`` ()=
    
    [<Test>] member t.
        ``When it opens a word file`` ()= 
            PopOpen.OpenW ((Path.Combine(Environment.CurrentDirectory, "Word.docx")), 10) |> should not' (equal 0)

    [<Test>] member t.
        ``When it opens the first excel file`` ()=
            PopOpen.OpenW ((Path.Combine(Environment.CurrentDirectory, "Excel 1.xlsx")), 10) |> should not' (equal 0)
            
    [<Test>] member t.
        ``When it opens the second excel file`` ()=
            PopOpen.OpenW ((Path.Combine(Environment.CurrentDirectory, "Excel 2.xlsx")), 10) |> should not' (equal 0)

    [<Test>] member t.
        ``When it opens the first powerpoint file`` ()=
            PopOpen.OpenW ((Path.Combine(Environment.CurrentDirectory, "Powerpoint 1.pptx")), 10) |> should not' (equal 0)

    [<Test>] member t.
        ``When it opens the second powerpoint file`` ()=
            PopOpen.OpenW ((Path.Combine(Environment.CurrentDirectory, "Powerpoint 2.pptx")), 10) |> should not' (equal 0)
    
    [<Test>] member t.
        ``When it opens a text file`` ()=
            PopOpen.OpenW ((Path.Combine(Environment.CurrentDirectory, "IAmTextFile.txt")), 10) |> should not' (equal 0)

    [<Test>] member t.
        ``When it opens a picture`` ()=
            PopOpen.OpenW ((Path.Combine(Environment.CurrentDirectory, "Picture.jpg")), 10) |> should not' (equal 0)
