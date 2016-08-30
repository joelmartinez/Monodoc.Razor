namespace Monodoc.Razor
    open System
    open System.Collections.Generic
    open System.Linq
    open System.Xml
    open System.Xml.Linq
    open System.Xml.XPath

    type EcmaModel(doc:XDocument, context:Dictionary<string,string>, xpath:string) =
        let index = match context.["show"] with
                        | "member" -> Int32.Parse context.["index"]
                        | _ -> 0

        member this.Source = doc
        member this.Context = context
        member this.XPath = xpath

        member this.Doc with get () = match this.XPath with 
                                        | x when x= "/" -> this.Source.Root
                                        | _ -> this.Source
                                                    .XPathSelectElements(this.XPath)
                                                    .Skip(index)
                                                    .First() 

                                                
        member this.Summary with get () = this.Doc.XPathSelectElement("Summary|summary")
        member this.Remarks with get () = this.Doc.XPathSelectElement("Remarks|remarks")

        member this.TypeMembers with get() = this.Source.XPathSelectElements("//Member")