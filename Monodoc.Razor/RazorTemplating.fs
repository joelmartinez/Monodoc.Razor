namespace Monodoc.Razor

    open System
    open System.Collections.Generic
    open System.Linq
    open System.Xml.Linq
    open System.Xml.XPath
    open RazorEngine.Templating
    open RazorEngine.Configuration

    type Templates = 
        | Namespace=0 
        | Type=1 
        | Class=2 
        | Member=3
        | Summary=4
        | Remarks=5

    type RazorTemplateBase<'T when 'T :> EcmaModel>() =
        inherit TemplateBase()

        let templateMap = new Dictionary<string, Func<TemplateWriter>>()
        [<DefaultValue>] val mutable public Model:'T
        [<DefaultValue>] val mutable public currentContext:XElement 

        member public this.Html = RazorHtmlHelper(this)

        override this.SetModel model =
            this.Model <- model :?> 'T
            base.SetModel(model)

        override this.ResolveLayout(name:string) =
            let l = base.ResolveLayout(name) 
            let ecmaModel = l :?> RazorTemplateBase<EcmaModel>
            ecmaModel.Model <- this.Model
            l

        member this.Match xpath delegateReference =
            templateMap.Add(xpath, delegateReference)
            ""
        member this.getTemplates = templateMap


    and public RazorHtmlHelper<'T when 'T :> EcmaModel>(template:RazorTemplateBase<'T>) =
        member this.Template = template

        member this.RenderPartial (templateName:Templates) =
            this.RenderPartial (templateName.ToString())

        member this.RenderPartial (templateName:string) =
            this.Template.Include(this.Template.Model.Context.["renderer-id"] + templateName, this.Template.Model)

        member this.RenderPartial (templateName:string, model:Object) =
            this.Template.Include(templateName, model)


    type public RazorTemplateBase() = 
        inherit RazorTemplateBase<EcmaModel>()

        let join (seq:IEnumerable<string>) = System.String.Join(Environment.NewLine, seq |> Seq.toArray)

        static member Initialize = 
            let config = new TemplateServiceConfiguration()
            config.Namespaces <- new HashSet<string>([|"Monodoc.Razor"; "System.Xml.Linq"; "System.Xml.XPath"; "System.Linq" |])
            config.BaseTemplateType <- typeof<RazorTemplateBase>

            let service = RazorEngineService.Create(config)
            service


