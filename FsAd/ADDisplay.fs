namespace FsAd

open FsAd
open FsAd.ADModule

module ADDisplay =

    let printGroup (group : ADGroup) =
        printfn "%s" group.FullName
        printfn "---------------------"

        group.Members
        |> List.iter (printfn "%s")

        printfn ""
