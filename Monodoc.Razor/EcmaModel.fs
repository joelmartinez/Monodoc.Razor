﻿namespace Monodoc.Razor
    open System
    open System.Collections.Generic
    open System.Linq
    open System.Xml
    open System.Xml.Linq
    open System.Xml.XPath

    open HtmlTranslator

    type EcmaModel(doc:XDocument, context:Dictionary<string,string>, xpath:string) =
        let index = match context.["show"] with
                        | "member" -> Int32.Parse context.["index"]
                        | _ -> 0

        let getSummary (e:XElement) = html (e.XPathSelectElement("Summary|summary|Docs/summary|Docs/Summary"))
        let getRemarks (e:XElement) = html (e.XPathSelectElement("Remarks|remarks|Docs/remarks|Docs/Remarks"))

        member this.Source = doc
        member this.Context = context
        member this.XPath = xpath

        member this.Doc with get () = match this.XPath with 
                                        | x when x= "/" -> this.Source.Root
                                        | _ -> this.Source
                                                    .XPathSelectElements(this.XPath)
                                                    .Skip(index)
                                                    .First() 

        member this.RenderSignature(name:string) =
            //let baseTypeName = this.Source.XPathSelectElement("Base/BaseTypeName").Value
            //let isStruct = if baseTypeName = "System.ValueType" then true else false
            let xpath = sprintf "TypeSignature[@Language='%s']|MemberSignature[@Language='%s']" name name
            this.Doc.XPathSelectElement(xpath).Attribute(XName.op_Implicit "Value").Value // TODO: render with links from this.Doc
                                                
        member this.Summary with get () = getSummary this.Doc
        member this.Remarks with get () = getRemarks this.Doc

        member this.TypeMembers with get() = this.Source.XPathSelectElements("//Member")
        member this.MemberParameters with get() = this.Doc.XPathSelectElements("Parameters/Parameter")

        member this.ParameterRemarks(name:string) = 
            ("//param[@name='"+ name) + "']"  |> this.Doc.XPathSelectElement |> html