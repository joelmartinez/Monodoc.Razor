namespace Monodoc.Razor 
    open Monodoc
    open System.Collections.Generic
    open System.Xml
    open System.Xml.Linq



    type public RazorGenerator =


        new () =
            RazorTemplateBase.Initialize
            new RazorGenerator()


        interface IDocGenerator<string> with
            member this.Generate ((hs:HelpSource), (internalId:string), (context:Dictionary<string, string>)) =

                let findTemplate (id:string) =
                    if id.Contains("T:") then "type" else "namespace"

                let source = hs.GetText internalId

                Renderer.transform (findTemplate internalId) source


