namespace Monodoc.Razor

    open System
    open System.Collections.Generic
    open System.Linq
    open System.Xml.Linq
    open System.Xml.XPath
    open RazorEngine.Templating
    open RazorEngine.Configuration


    type EcmaModel(doc:XDocument, context:Dictionary<string,string>, xpath:string) =
        let index = match context.["show"] with
                        | "member" -> Int32.Parse context.["index"]
                        | _ -> 0

        member this.Source = doc
        member this.Context = context
        member this.XPath = xpath

        member this.Doc with get () = this.Source.XPathSelectElements(this.XPath).Skip(index).First()

    type RazorTemplateBase<'T>() =
        inherit TemplateBase()

        let templateMap = new Dictionary<string, Func<TemplateWriter>>()
        [<DefaultValue>] val mutable public Model:'T
        [<DefaultValue>] val mutable public currentContext:XElement 

        override this.SetModel model =
            this.Model <- model :?> 'T
            base.SetModel(model)

        member this.Match xpath delegateReference =
            templateMap.Add(xpath, delegateReference)
            ""
        member this.getTemplates = templateMap


    type public RazorTemplateBase() = 
        inherit RazorTemplateBase<EcmaModel>()

        let join (seq:IEnumerable<string>) = System.String.Join(Environment.NewLine, seq |> Seq.toArray)

        static member Initialize = 
            let config = new TemplateServiceConfiguration()
            config.Namespaces <- new HashSet<string>([|"Monodoc.Razor"; "System.Xml.Linq"|])

            let service = RazorEngineService.Create(config)
            service
