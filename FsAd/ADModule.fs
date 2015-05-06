namespace FsAd

open System
open System.DirectoryServices

module DirectoryServiceExtensions =
    type DirectorySearcher with
        member this.addPropertiesToLoad(properties) =
            this.PropertiesToLoad.AddRange(properties) |> ignore

    type SearchResult with
        member this.Item
            with get(name) = match this.Properties.[name].Count with
                             | 0 -> None
                             | _ -> Some <| this.Properties.[name].[0].ToString ()

    type DirectoryEntry with
        member this.Item
            with get(name) = this.Properties.[name].[0].ToString ()

type ADUser = {
    FullName: string option
    EmailAddress: string option
    FirstName: string option
    LastName: string option
    LoginName: string option
    DistinguishedName: string option
    }

open DirectoryServiceExtensions

module ADModule =

    let getCurrentDomainPath _ =
        use de = new DirectoryEntry "LDAP://RootDSE"
        "LDAP://" + de.["defaultNamingContext"]

    let getAllUsers limit domainPath =
        use de = new DirectoryEntry(domainPath)
        use ds = new DirectorySearcher(de)
        ds.Filter <- "(&(objectCategory=User)(objectClass=person))"
        if limit > 0 then
            ds.SizeLimit <- limit
        
        let results = ds.FindAll ()

        [ for sr in results -> sr.["name"] ]

    /// Returns a disposable DirectorySearcher
    let buildUserSearcher (de : DirectoryEntry) =
        let ds = new DirectorySearcher(de)
        ds.addPropertiesToLoad [| "name"; "mail"; "givenname"; "sn"; "userPrincipalName"; "distinguishedName" |]
        ds

    let adUser (sr : SearchResult) =
      { FullName = sr.["name"]
        EmailAddress = sr.["mail"]
        FirstName = sr.["givenname"]
        LastName = sr.["sn"]
        LoginName = sr.["userPrincipalName"]
        DistinguishedName = sr.["distinguishedName"]
      }


    let getAdditionalUserInformation limit domainPath =
        use de = new DirectoryEntry(domainPath)
        use ds = buildUserSearcher de

        ds.addPropertiesToLoad [| "name"; "mail"; "givenname"; "sn"; "userPrincipalName"; "distinguishedName" |]
      
        ds.Filter <- "(&(objectCategory=User)(objectClass=person))"
        if limit > 0 then
            ds.SizeLimit <- limit

        [ for sr in ds.FindAll () -> adUser sr ]

    let searchForUsers domainPath userName =
        use de = new DirectoryEntry(domainPath)
        use ds = buildUserSearcher de

        ds.Filter <- sprintf "(&(objectCategory=User)(objectClass=person)(name=%s*))" userName

        [ for sr in ds.FindAll () -> adUser sr ]

    let getAUser domainPath userName =
        use de = new DirectoryEntry(domainPath)
        use ds = buildUserSearcher de

        ds.Filter <- sprintf "(&(objectCategory=User)(objectClass=person)(name=%s))" userName
        
        let sr = ds.FindOne ()

        if sr = null then
            None
        else
            Some <| adUser sr


