namespace FsAd

open System.DirectoryServices
open FsAd.ADModule
open FsAd.DirectoryServiceExtensions

type ADDomain (path) =
    new() = ADDomain(getCurrentDomainPath ())

    member __.path = path

    member __.findAllUserFullNames limit =
        use de = new DirectoryEntry(path)
        use ds = new DirectorySearcher(de)
        ds.Filter <- "(&(objectCategory=User)(objectClass=person))"
        if limit > 0 then
            ds.SizeLimit <- limit

        [ for sr in ds.FindAll () -> sr.FullName ]

    member __.findAllUsers limit =
        userQuery path limit "(&(objectCategory=User)(objectClass=person))" ADUser.ofSearchResult

    member __.findUsersByName limit name =
        userQuery path limit (sprintf "(&(objectCategory=User)(objectClass=person)(name=%s*))" name) ADUser.ofSearchResult

    member __.findUserByName name =
        use de = new DirectoryEntry(path)
        use ds = buildUserSearcher 1 de
        ds.Filter <- sprintf "(&(objectCategory=User)(objectClass=person)(name=%s))" name
        ds.findAndMapOne ADUser.ofSearchResult

    member __.findAllGroups limit =
        groupQuery path limit "(&(objectCategory=Group))" ADGroup.ofSearchResult

    member __.findGroupsByName limit name =
        groupQuery path limit (sprintf "(&(objectCategory=Group)(name=*%s*))" name) ADGroup.ofSearchResult

    member __.findGroup name =
        use de = new DirectoryEntry(path)
        use ds = buildGroupSearcher 1 de
        ds.Filter <- sprintf "(&(objectCategory=Group)(name=%s))" name
        ds.findAndMapOne ADGroup.ofSearchResult
