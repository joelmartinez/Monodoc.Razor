namespace Monodoc.Razor
    open System.Collections.Generic
    open System.Xml.Linq
    open RazorEngine
    open RazorEngine.Templating

    type RazorRenderer() =
        let engine = RazorTemplateBase.Initialize
        let id = System.Guid.NewGuid().ToString()

        member x.add (name:string) (template:string) = 
            engine.Compile(template, id+name, typeof<EcmaModel>)

        member x.transform (name:string) (xml:string) (context:Dictionary<string,string>) =
            let doc = XDocument.Parse(xml)

            let model = EcmaModel(doc, context)

            engine.Run(id+name, typeof<EcmaModel>, model).Trim()