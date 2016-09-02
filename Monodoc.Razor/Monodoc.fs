namespace Monodoc.Razor 
    open Monodoc
    open System.Collections.Generic
    open System.Xml
    open System.Xml.Linq



    type public RazorGenerator() =
        let renderer = RazorRenderer()

        member x.Add (name:Templates) template = 
            renderer.add (name, template)

        interface IDocGenerator<string> with
            member this.Generate ((hs:HelpSource), (internalId:string), (context:Dictionary<string, string>)) =

                let findTemplate (id:string) =
                    context.["show"]

                let source = hs.GetText internalId

                let docPath = match context.["show"] with
                                | "member" -> sprintf "//Member[MemberType='%s']" context.["membertype"]
                                | _ -> "/"

                renderer.transform (findTemplate internalId) source context docPath


