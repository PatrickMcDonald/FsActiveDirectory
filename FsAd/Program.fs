/// F# Source code for http://www.codemag.com/Article/1312041

open System
open FsAd
open FsAd.ADModule
open FsAd.ADDisplay

[<EntryPoint>]
let main argv =

    let currentDomain = ADDomain ()

    currentDomain.path |> printfn "Domain Path: %s"
    printfn ""

    let limit1 = 10 in
    printfn "Retrieving first %d user names" limit1
    currentDomain.findAllUserFullNames limit1 |> List.iter (printfn "%A")
    printfn ""

//    let limit2 = 20 in
//    printfn "Retrieving first %d users" limit2
//    currentDomain.findAllUsers limit2 |> List.iter (printfn "%A")
//    printfn ""

//    let userName1 = "mcdonald" in
//    printfn "Search for %s" userName1
//    currentDomain.findUsersByName 100 userName1 |> List.iter (printfn "%A")
//    printfn ""

    let userName2 = "mcdonald, patrick" in
    printfn "Retrieving user %s" userName2
    currentDomain.findUserByName userName2 |> printfn "%A"
    printfn ""

//    currentDomain.findAllGroups 10 |> printfn "%A"
//    printfn ""

    currentDomain.findGroupsByName 100 "contacthistory" |> List.iter printGroup
    printfn ""

//    ["GIT_G_045_Mig_Prod_Factsheets"; "GIT_G_045_Product_Launches_Information"; "GIT_G_045_CUSTOMER_ISSUES"]
//    |> List.collect (currentDomain.findGroupsByName 100)
//    |> printfn "%A"


    printf "Press any key to continue . . . "
    Console.ReadKey true |> ignore

    0 // return an integer exit code
