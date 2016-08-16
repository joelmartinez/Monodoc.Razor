namespace Monodoc.Razor
    open System.Collections.Generic
    open System.Xml.Linq
    open RazorEngine
    open RazorEngine.Templating

    type Templates = Namespace=0 | Type=1 | Class=2 

    type RazorRenderer() =
        let engine = RazorTemplateBase.Initialize
        let id = System.Guid.NewGuid().ToString()

        let template_name (ttype:Templates) = 
            match ttype with
                | Templates.Namespace -> "namespace"
                | Templates.Type -> "typeoverview"
                | Templates.Class -> "class"


        member x.add (name:Templates) (template:string) = 
            engine.Compile(template, (id+(template_name name)), typeof<EcmaModel>)

        member x.transform (name:string) (xml:string) (context:Dictionary<string,string>) =
            let doc = XDocument.Parse(xml)

            let model = EcmaModel(doc, context)

            engine.Run(id+name, typeof<EcmaModel>, model).Trim()