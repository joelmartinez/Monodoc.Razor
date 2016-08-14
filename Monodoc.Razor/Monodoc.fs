namespace Monodoc.Razor 
    open Monodoc
    open System.Collections.Generic
    open System.Xml
    open System.Xml.Linq



    type public RazorGenerator() =

        member x.Initialize() =
            RazorTemplateBase.Initialize
        
        member x.Add name template = 
            Renderer.add name template

        interface IDocGenerator<string> with
            member this.Generate ((hs:HelpSource), (internalId:string), (context:Dictionary<string, string>)) =

                let findTemplate (id:string) =
                    context.["show"]

                let source = hs.GetText internalId

                Renderer.transform (findTemplate internalId) source


