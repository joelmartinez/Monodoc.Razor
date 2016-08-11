module public Monodoc.Razor.Renderer 
    open System.Xml.Linq
    open RazorEngine
    open RazorEngine.Templating
    let add (name:string) (template:string) = 
        Engine.Razor.Compile(template, name, typeof<XDocument>)

    let transform (name:string) (xml:string) =
        let doc = XDocument.Parse(xml)

        Engine.Razor.Run(name, typeof<XDocument>, doc)