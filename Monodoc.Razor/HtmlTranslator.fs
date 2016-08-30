namespace Monodoc.Razor
    open System
    open System.Xml
    open System.Xml.Linq
    open System.Xml.XPath

    module HtmlTranslator =
    
        let html (doc:XElement)  = doc.ToString()
