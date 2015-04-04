﻿namespace PopFs.Tests

open NUnit.Framework
open FsUnit
open Pop
open System
open System.IO

[<TestFixture>]
type ``Given PopOpener`` ()=
    
    [<Test>] member t.
        ``When it opens`` ()= 
            PopOpen.Open (Path.Combine(Environment.CurrentDirectory, "Word.docx")) |> should not' (equal 0)

    [<Test>] member t.
        ``When it opens the first excel file`` ()=
            PopOpen.Open (Path.Combine(Environment.CurrentDirectory, "Excel 1.xlsx")) |> should not' (equal 0)
            
    [<Test>] member t.
        ``When it opens the second excel file`` ()=
            PopOpen.Open (Path.Combine(Environment.CurrentDirectory, "Excel 2.xlsx")) |> should not' (equal 0)

    [<Test>] member t.
        ``When it opens the first powerpoint file`` ()=
            PopOpen.Open (Path.Combine(Environment.CurrentDirectory, "Powerpoint 1.pptx")) |> should not' (equal 0)

    [<Test>] member t.
        ``When it opens the second powerpoint file`` ()=
            PopOpen.Open (Path.Combine(Environment.CurrentDirectory, "Powerpoint 2.pptx")) |> should not' (equal 0)
    
    [<Test>] member t.
        ``When it opens a text file`` ()=
            PopOpen.Open (Path.Combine(Environment.CurrentDirectory, "IAmTextFile.txt")) |> should not' (equal 0)

    [<Test>] member t.
        ``When it opens a picture`` ()=
            PopOpen.Open (Path.Combine(Environment.CurrentDirectory, "Picture.jpg")) |> should not' (equal 0)
