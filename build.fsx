#r @"packages/FAKE.3.23.0/tools/FakeLib.dll"
open Fake

let buildDir = "./build/"
let packageDir = "./packaging/"

Target "Clean" (fun _ ->
    trace "Cleaning up the build directory" 
    CleanDir buildDir
)

Target "BuildApp" (fun _ ->
    trace "Building ..."
    !! "./Pop/*.fsproj"
        |> MSBuildRelease buildDir "Build"
        |> Log "AppBuild-Output"
)

Target "Default" (fun _ -> 
    trace "The build is complete"
)

//Target "CreatePackage" (fun _ ->
    //CopyFiles packageDir allPackageFiles
//)

"Clean"
==> "BuildApp"
==> "Default"

RunTargetOrDefault "Default"