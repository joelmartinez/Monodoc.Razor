namespace Monodoc.Razor.Tests
open System
open NUnit.Framework
open Monodoc
open Monodoc.Razor

[<TestFixture>]
type Test() = 

    [<Test>]
    member x.RendererUsesTemplate() =
        RazorTemplateBase.Initialize
        Renderer.add "type" "rendered"
        let actual = Renderer.transform "type" "<Type></Type>"

        Assert.AreEqual ("rendered", actual)


    [<Test>]
    member x.RendererUsedViaMonodoc() =
        // Initialize the generator
        let generator = new RazorGenerator()
        generator.Initialize()

        let typeTemplate = "@inherits RazorTemplateBase
        @Model.Element(\"Type\").Attribute(\"Name\").Value"

        generator.Add "typeoverview" typeTemplate

        // assumes the `make assemble` has been run
        let assemblyPath = typedefof<Test>
        let contentPath = IO.Path.GetDirectoryName(assemblyPath.Assembly.Location)
        let tree = RootTree.LoadTree(contentPath, true)

        let renderedOutput = tree.RenderUrl("T:My.Sample.SomeClass", generator)

        Assert.AreEqual("SomeClass", renderedOutput.Trim())
