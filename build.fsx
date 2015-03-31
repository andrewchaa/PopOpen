#r @"packages/FAKE.3.23.0/tools/FakeLib.dll"
open Fake

let buildDir = "./build/"
let nugetDir = "./nuget"
let deployDir = "./Publish"
let packagesDir = "./packages/"

Target "Clean" (fun _ ->
    trace "Cleaning up the build directory" 
    CleanDirs [buildDir; deployDir;]
)

Target "BuildApp" (fun _ ->
    trace "Building ..."
    !! "./Pop/*.fsproj"
        |> MSBuildRelease buildDir "Build"
        |> Log "Build-Output"
)

Target "Default" (fun _ -> 
    trace "The build is complete"
)

Target "CreatePackage" (fun _ ->
    let allPackageFiles = [
        "./build/FSharp.Core.dll"; 
        "./build/Pop.Cs.dll";
        "./build/Pop.dll"
        ] 

    CopyFiles nugetDir allPackageFiles

    "Pop.nuspec"
    |> NuGet (fun p -> 
        { 
        p with 
            Authors = ["Andrew Chaa"]
            Version = "0.8.3.1"
            NoPackageAnalysis = true
            ToolPath = @".\Nuget.exe" 
            OutputPath = nugetDir
            Publish = false 
        })
        
)

"Clean"
==> "BuildApp"
==> "CreatePackage"
==> "Default"

RunTargetOrDefault "Default"