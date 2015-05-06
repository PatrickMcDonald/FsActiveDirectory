// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open System
open FsAd.ADModule

[<EntryPoint>]
let main argv =

    let domainPath = getCurrentDomainPath ()

    domainPath |> printfn "%s"

//    let limit1 = 10 in
//    printfn "Retrieving first %d user names" limit1
//    getAllUsers limit1 domainPath |> List.iter (printfn "%A")
//    printfn ""

//    let limit2 = 20 in
//    printfn "Retrieving first %d users" limit2
//    getAdditionalUserInformation limit2 domainPath |> List.iter (printfn "%A")
//    printfn ""

//    let userName1 = "mcdonald" in
//    printfn "Search for %s" userName1
//    searchForUsers domainPath userName1 |> List.iter (printfn "%A")
//    printfn ""


//    getAUser domainPath "McDonald, Patrick" |> printfn "%A"

    
    printfn ""
    printf "Press any key to continue . . . "
    Console.ReadKey true |> ignore

    0 // return an integer exit code
