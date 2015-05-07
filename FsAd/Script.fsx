#r "System.DirectoryServices.dll"

#load "ADModule.fs"

open System
open FsAd.ADModule

let domainPath = getCurrentDomainPath ()

findAllUserFullNames 10 domainPath

findAllUsers 0 domainPath

findUsersByName 100 domainPath "mcdonald"

findUserByName domainPath "mcdonald, patrick"

findAllGroups 10 domainPath

findGroupsByName 100 domainPath "GIT_G_045_CUSTOMER_ISSUES"

findGroup domainPath "GIT_G_045_CUSTOMER_ISSUES"

authenticateUser "DC=domain,DC=internal" "redacted" "password"
