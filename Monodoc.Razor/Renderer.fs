namespace Monodoc.Razor
    open System.Collections.Generic
    open System.Xml.Linq
    open RazorEngine
    open RazorEngine.Templating



    type RazorRenderer() =
        let engine = RazorTemplateBase.Initialize
        let id = System.Guid.NewGuid().ToString()

        let template_name (ttype:Templates) = 
            match ttype with
                | Templates.Namespace -> "namespace"
                | Templates.Type -> "typeoverview"
                | Templates.Class -> "class"
                | Templates.Member -> "member"
                | _ -> ttype.ToString()

        
        member x.add (name:Templates, template:string) = 
            x.add (template_name name, template)

        member x.add (name:string, template:string) =
            engine.Compile(template, (id+name), typeof<EcmaModel>)

        member x.transform (name:string) (xml:string) (context:Dictionary<string,string>) (xpath:string) =
            let doc = XDocument.Parse(xml)
            context.["renderer-id"] <- id
            let model = EcmaModel(doc, context, xpath)

            engine.Run(id+name, typeof<EcmaModel>, model).Trim()