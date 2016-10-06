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
            let renderNodes nodes =
                nodes |> Seq.iter (fun d -> render d sb)

            match doc with
            | :? XElement as e -> 
                match e.Name.LocalName with
                // TODO: include logic for individual constructs in ecma XML: http://docs.go-mono.com/?link=man%3amdoc(5)
                | "para" -> 
                    sb.Append("<p>") |> ignore
                    e.Nodes() |> renderNodes
                    sb.Append("</p>") |> ignore
                | "block" ->
                    sb.Append("<div>") |> ignore

                    match e.Attribute(XName.op_Implicit("type")) with
                    | null -> ()
                    | t -> 
                        match t.Value with
                        | "behaviors" -> sb.Append("<h3>Operation</h3>") |> ignore
                        | _ -> ()
                    
                    e.Nodes() |> renderNodes
                    sb.Append("</div>") |> ignore
                | _ -> e.Nodes() |> renderNodes

            | :? XText as t -> sb.Append(t.Value) |> ignore 
            | _ -> failwith "unknown node type"

        let html doc  =
            if doc = null then
                String.Empty
            else
                let sb = StringBuilder()
                render doc sb

                let finalResult = sb.ToString()

                if finalResult = "To be added." then
                    String.Empty
                else
                    finalResult
