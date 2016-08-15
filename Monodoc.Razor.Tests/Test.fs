namespace Monodoc.Razor.Tests
open System
open NUnit.Framework
open Monodoc
open Monodoc.Razor

[<TestFixture>]
type Test() = 

    let getGenerator = 
        let generator = new RazorGenerator()
        generator.Initialize()
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
        RazorTemplateBase.Initialize
        Renderer.add "type" "rendered"
        let actual = Renderer.transform "type" "<Type></Type>"

        Assert.AreEqual ("rendered", actual)


    [<Test>]
    member x.RendererUsedViaMonodoc() =
        let generator = getGenerator

        let typeTemplate = "@inherits RazorTemplateBase
        @Model.Element(\"Type\").Attribute(\"Name\").Value"

        generator.Add "typeoverview" typeTemplate

        let renderedOutput = tree.RenderUrl("T:My.Sample.SomeClass", generator)

        Assert.AreEqual("SomeClass", renderedOutput)
