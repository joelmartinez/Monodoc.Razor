namespace Monodoc.Razor.Tests
open System
open NUnit.Framework
open Monodoc.Razor

[<TestFixture>]
type Test() = 

    [<Test>]
    member x.TestCase() =
        Renderer.add "" ""
        Assert.IsTrue(false)

