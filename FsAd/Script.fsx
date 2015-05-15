#r "System.DirectoryServices.dll"

#load "ADModule.fs"
#load "ADDisplay.fs"

open System
open FsAd.ADModule
open FsAd.ADDisplay

let domainPath = getCurrentDomainPath ()

findAllUserFullNames 10 domainPath

findAllUsers 0 domainPath

findUsersByName 100 domainPath "mcdonald"

findUserByName domainPath "mcdonald, patrick"

findAllGroups 10 domainPath

findGroupsByName 100 domainPath "GIT_GS_SEC_Retail_ContactHistory"
|> List.iter printGroup

findGroup domainPath "GIT_GS_SEC_Retail_ContactHistory"
|> Option.iter printGroup

authenticateUser "DC=domain,DC=internal" "redacted" "password"
