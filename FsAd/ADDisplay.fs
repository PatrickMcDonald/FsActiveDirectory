namespace FsAd

open FsAd
open FsAd.ADModule

module ADDisplay =

    let printGroup (group : ADGroup) =
        printfn "%s" group.FullName
        printfn "%s" <| System.String ('-', group.FullName.Length)

        group.Members
        |> List.iter (printfn "%s")

        printfn ""
