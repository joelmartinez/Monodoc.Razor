namespace Monodoc.Razor 
    open Monodoc
    open System.Collections.Generic

    type RazorGenerator() =
        interface IDocGenerator<string> with
            member this.Generate ((hs:HelpSource), (internalId:string), (context:Dictionary<string, string>)) =
                hs.GetText internalId
