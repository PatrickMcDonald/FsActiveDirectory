namespace FsAd

open System
open System.DirectoryServices

module List =
    let ofEnumerable<'T> collection =
        collection
        |> Seq.cast<'T>
        |> List.ofSeq

module DirectoryServiceExtensions =

    type DirectorySearcher with
        member this.addPropertiesToLoad(properties) =
            this.PropertiesToLoad.AddRange(Array.ofSeq properties) |> ignore

        member this.findAndMap mapping =
            this.FindAll ()
            |> List.ofEnumerable<SearchResult>
            |> List.map mapping

        member this.findAndMapOne mapping =
            match this.FindOne() with
            | null -> None
            | sr ->   Some <| mapping sr

    let private propertyNotFound name = sprintf "\"%s\" property not found" name

    type SearchResult with
        member this.Get(name) =
            if this.Properties.[name].Count = 0 then raise <| InvalidOperationException (propertyNotFound name)
            this.Properties.[name].[0].ToString ()

        member this.FullName = this.Get "name"

        member this.Item
            with get(name) = match this.Properties.[name].Count with
                             | 0 -> None
                             | _ -> Some <| this.Properties.[name].[0].ToString ()

    type DirectoryEntry with
        member this.DefaultNamingContext =
            let propertyName = "defaultNamingContext"
            if this.Properties.[propertyName].Count = 0 then raise <| InvalidOperationException (propertyNotFound propertyName)
            this.Properties.[propertyName].[0].ToString ()

type ADUser = {
    FullName: string
    EmailAddress: string option
    FirstName: string option
    LastName: string option
    LoginName: string option
    DistinguishedName: string option
    LogonNamePreWindows2000: string option
    }

type ADGroup = {
    FullName: string
    MemberOf: string list
    Members: string list
    }

open DirectoryServiceExtensions

module ADModule =
    let getCurrentDomainPath _ =
        use de = new DirectoryEntry "LDAP://RootDSE"
        "LDAP://" + de.DefaultNamingContext

    /// Returns a disposable DirectorySearcher
    let buildUserSearcher limit (de : DirectoryEntry) =
        let ds = new DirectorySearcher(de)
        ds.addPropertiesToLoad ["name"; "mail"; "givenname"; "sn"; "userPrincipalName"; "distinguishedName"; "sAMAccountName"]
        ds.Sort <- SortOption("name", SortDirection.Ascending)
        if limit > 0 then
            ds.SizeLimit <- limit
        ds

    /// Returns a disposable DirectorySearcher
    let buildGroupSearcher limit (de : DirectoryEntry) =
        let ds = new DirectorySearcher(de)
        ds.addPropertiesToLoad ["name"; "memberof"; "member"]
        ds.Sort <- SortOption("name", SortDirection.Ascending)
        if limit > 0 then
            ds.SizeLimit <- limit
        ds

    let adUser (sr : SearchResult) =
      { FullName = sr.FullName
        EmailAddress = sr.["mail"]
        FirstName = sr.["givenname"]
        LastName = sr.["sn"]
        LoginName = sr.["userPrincipalName"]
        DistinguishedName = sr.["distinguishedName"]
        LogonNamePreWindows2000 = sr.["sAMAccountName"]
      }

    let adGroup (sr: SearchResult) =
      { FullName = sr.FullName
        MemberOf = sr.Properties.["memberof"] |> Seq.cast<string> |> Seq.sort |> List.ofSeq
        Members = sr.Properties.["member"] |> Seq.cast<string> |> Seq.sort |> List.ofSeq
      }

    let userQuery domainPath limit filter mapping =
        use de = new DirectoryEntry(domainPath)
        use ds = buildUserSearcher limit de
        ds.Filter <- filter
        ds.findAndMap mapping

    let groupQuery domainPath limit filter mapping =
        use de = new DirectoryEntry(domainPath)
        use ds = buildGroupSearcher limit de
        ds.Filter <- filter
        ds.findAndMap mapping

    let authenticateUser domainName userName password =
        try
            use de = new DirectoryEntry("LDAP://" + domainName, userName, password)
            use ds = new DirectorySearcher(de)

            ds.FindOne () |> ignore

            true
        with
            | _ -> false
