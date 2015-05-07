/// F# Source code for http://www.codemag.com/Article/1312041

open System
open FsAd.ADModule

[<EntryPoint>]
let main argv =

    let domainPath = getCurrentDomainPath ()

    domainPath |> printfn "Domain Path: %s"
    printfn ""

    let limit1 = 10 in
    printfn "Retrieving first %d user names" limit1
    findAllUserFullNames limit1 domainPath |> List.iter (printfn "%A")
    printfn ""

//    let limit2 = 20 in
//    printfn "Retrieving first %d users" limit2
//    findAllUsers limit2 domainPath |> List.iter (printfn "%A")
//    printfn ""

//    let userName1 = "mcdonald" in
//    printfn "Search for %s" userName1
//    findUsersByName 100 domainPath userName1 |> List.iter (printfn "%A")
//    printfn ""

    let userName2 = "mcdonald, patrick" in
    printfn "Retrieving user %s" userName2
    findUserByName domainPath userName2 |> printfn "%A"
    printfn ""

//    findAllGroups 10 domainPath |> printfn "%A"
//    printfn ""

//    findGroupsByName 100 domainPath "contacthistory" |> printfn "%A"
//    printfn ""

//    ["GIT_G_045_Mig_Prod_Factsheets"; "GIT_G_045_Product_Launches_Information"; "GIT_G_045_CUSTOMER_ISSUES"]
//    |> List.collect (findGroupsByName 100 domainPath)
//    |> printfn "%A"


    printf "Press any key to continue . . . "
    Console.ReadKey true |> ignore

    0 // return an integer exit code
