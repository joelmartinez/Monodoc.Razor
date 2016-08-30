namespace Monodoc.Razor

    open System
    open System.Collections.Generic
    open System.Linq
    open System.Xml.Linq
    open System.Xml.XPath
    open RazorEngine.Templating
    open RazorEngine.Configuration



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
            config.Namespaces <- new HashSet<string>([|"Monodoc.Razor"; "System.Xml.Linq"; "System.Xml.XPath"; "System.Linq" |])

            let service = RazorEngineService.Create(config)
            service
