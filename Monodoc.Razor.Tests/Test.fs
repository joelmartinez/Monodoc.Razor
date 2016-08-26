namespace Monodoc.Razor.Tests
open System
open System.Collections.Generic
open System.IO

open NUnit.Framework
open Monodoc
open Monodoc.Razor

[<TestFixture>]
type Test() = 

    let getGenerator = 
        let generator = new RazorGenerator()
        generator

    let getTree = 
        // assumes the `make assemble` has been run
        let assemblyPath = typedefof<Test>
        let contentPath = IO.Path.GetDirectoryName(assemblyPath.Assembly.Location)
        let tree = RootTree.LoadTree(contentPath, true)
        tree

    let tree = getTree


    [<Test>]
    member x.RendererUsesTemplate() =

        let renderer = RazorRenderer()

        let context = Dictionary<string,string>()
        context.Add ("show", "type")

        renderer.add Templates.Type "rendered"
        let actual = renderer.transform "typeoverview" "<Type></Type>" context "/"

        Assert.AreEqual ("rendered", actual)

    
    [<Test>]
    member x.RendererUsedViaMonodoc() =
        let generator = getGenerator

        let typeTemplate = "@inherits RazorTemplateBase
        @Model.Source.Element(\"Type\").Attribute(\"Name\").Value"

        generator.Add Templates.Type typeTemplate

        let renderedOutput = tree.RenderUrl("T:My.Sample.SomeClass", generator)

        Assert.AreEqual("SomeClass", renderedOutput)


    [<Test>]
    member x.RendererUsedViaMonodoc_namespace() =
        let generator = getGenerator

        let typeTemplate = "@inherits RazorTemplateBase
        @Model.Context[\"namespace\"]"

        generator.Add Templates.Namespace typeTemplate

        let renderedOutput = tree.RenderUrl("N:My.Sample", generator)

        Assert.AreEqual("My.Sample", renderedOutput)

    [<Test>]
    member x.RendererUsedViaMonodoc_method() =
        let generator = getGenerator   

        let typeTemplate = "@inherits RazorTemplateBase
        @Raw(Model.Doc.Attribute(\"MemberName\").Value)"

        generator.Add Templates.Member typeTemplate

        let renderedOutput = tree.RenderUrl("M:My.Sample.SomeClass.GetName(string)", generator)

        Assert.AreEqual("GetName", renderedOutput)

    [<Test>]
    member x.FileNamespace() =
        let generator = getGenerator  

        let path = "../../../Templates/namespace.cshtml"
        let template = File.ReadAllText(path);

        generator.Add Templates.Namespace template

        let rendered = tree.RenderUrl("N:My.Sample", generator); 

        Assert.AreEqual(true, rendered.Contains("<h1>My.Sample"));