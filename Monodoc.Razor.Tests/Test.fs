namespace Monodoc.Razor.Tests
open System
open System.Collections.Generic
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
        renderer.add "type" "rendered"
        let actual = renderer.transform "type" "<Type></Type>" context

        Assert.AreEqual ("rendered", actual)

    
    [<Test>]
    member x.RendererUsedViaMonodoc() =
        let generator = getGenerator

        let typeTemplate = "@inherits RazorTemplateBase
        @Model.Source.Element(\"Type\").Attribute(\"Name\").Value"

        generator.Add "typeoverview" typeTemplate

        let renderedOutput = tree.RenderUrl("T:My.Sample.SomeClass", generator)

        Assert.AreEqual("SomeClass", renderedOutput)


    [<Test>]
    member x.RendererUsedViaMonodoc_namespace() =
        let generator = getGenerator

        let typeTemplate = "@inherits RazorTemplateBase
        @Model.Context[\"namespace\"]"

        generator.Add "namespace" typeTemplate

        let renderedOutput = tree.RenderUrl("N:My.Sample", generator)

        Assert.AreEqual("My.Sample", renderedOutput)
