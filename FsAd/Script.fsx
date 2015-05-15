#load "FsAd.fsx"

open System
open FsAd
open FsAd.ADDisplay

let currentDomain = FsAd.ADDomain()

currentDomain.findAllUserFullNames 10

currentDomain.findAllUsers 0

currentDomain.findUsersByName 100 "mcdonald"

currentDomain.findUserByName "mcdonald, patrick"

currentDomain.findAllGroups 10

currentDomain.findGroupsByName 100 "GIT_GS_SEC_Retail_ContactHistory"
|> List.iter printGroup

currentDomain.findGroup "GIT_GS_SEC_Retail_ContactHistory"
|> Option.iter printGroup

FsAd.ADModule.authenticateUser "DC=domain,DC=internal" "redacted" "password"
