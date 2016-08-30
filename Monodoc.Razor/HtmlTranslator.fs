namespace Monodoc.Razor
    open System
    open System.Collections.Generic
    open System.Text
    open System.Linq
    open System.Xml
    open System.Xml.Linq
    open System.Xml.XPath

    module HtmlTranslator =
        let rec render (doc:XObject) (sb:StringBuilder) =
            let renderNodes (nodes:IEnumerable<XNode>) =
                nodes |> Seq.iter (fun d -> render d sb)

            match doc with
            | :? XElement as e -> 
                match e.Name.LocalName with
                // TODO: include logic for individual constructs in ecma XML
                | "para" -> 
                    sb.Append("<p>") |> ignore
                    e.Nodes() |> renderNodes
                    sb.Append("</p>") |> ignore
                | _ -> e.Nodes() |> renderNodes

            | :? XText as t -> sb.Append(t.Value) |> ignore 
            | _ -> failwith "unknown node type"

        let html (doc:XElement)  = 
            let sb = StringBuilder()
            render doc sb

            sb.ToString()
