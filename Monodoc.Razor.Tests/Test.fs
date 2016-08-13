namespace Monodoc.Razor.Tests
open System
open NUnit.Framework
open Monodoc.Razor

[<TestFixture>]
type Test() = 

    [<Test>]
    member x.RendererUsesTemplate() =
        RazorTemplateBase.Initialize
        Renderer.add "type" "rendered"
        let actual = Renderer.transform "type" "<Type></Type>"

        Assert.AreEqual ("rendered", actual)

