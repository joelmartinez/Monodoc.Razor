namespace Monodoc.Razor.Tests
open System
open System.Collections.Generic
open System.IO

open NUnit.Framework
open Monodoc
open Monodoc.Razor

[<TestFixture>]
type Test() = 

    let getGenerator() = 
        let generator = new RazorGenerator()
        generator

    let getTree = 
        // assumes the `make assemble` has been run
        let assemblyPath = typedefof<Test>
        let contentPath = IO.Path.GetDirectoryName(assemblyPath.Assembly.Location)
        let tree = RootTree.LoadTree(contentPath, true)
        tree

    let tree = getTree

    let (@@) a b = Path.Combine(a,b)

    let getTemplate path = "../../../Templates" @@ path + ".cshtml" |> File.ReadAllText

    let loadShared (generator:RazorGenerator) = 
        let summarytemplate = "Shared" @@ "summary" |> getTemplate
        let remarkstemplate = "Shared" @@ "remarks" |> getTemplate
        generator.Add Templates.Summary summarytemplate
        generator.Add Templates.Remarks remarkstemplate

    let loadNamespace (generator:RazorGenerator) = 
        "namespace" |> getTemplate |> generator.Add Templates.Namespace 
        generator

    let loadType (generator:RazorGenerator) = 
        "typeoverview" |> getTemplate |> generator.Add Templates.Type 
        generator

    
    let loadMember (generator:RazorGenerator) = 
        "member" |> getTemplate |> generator.Add Templates.Member 
        generator

    [<Test>]
    member x.RendererUsesTemplate() =

        let renderer = RazorRenderer()

        let context = Dictionary<string,string>()
        context.Add ("show", "type")

        renderer.add (Templates.Type, "@inherits RazorTemplateBase 
        rendered")
        let actual = renderer.transform "typeoverview" "<Type></Type>" context "/"

        Assert.AreEqual ("rendered", actual)


    [<Test>]
    member x.RendererPartials() =

        let renderer = RazorRenderer()

        let context = Dictionary<string,string>()
        context.Add ("show", "type")

        renderer.add ("subtype", "partial")
        renderer.add (Templates.Type, "rendered @Html.RenderPartial(\"subtype\")")
        let actual = renderer.transform "typeoverview" "<Type></Type>" context "/"

        Assert.AreEqual ("rendered partial", actual)

    
    [<Test>]
    member x.RendererUsedViaMonodoc() =
        let generator = getGenerator()

        let typeTemplate = "@inherits RazorTemplateBase
        @Model.Source.Element(\"Type\").Attribute(\"Name\").Value"

        generator.Add Templates.Type typeTemplate

        let renderedOutput = tree.RenderUrl("T:My.Sample.SomeClass", generator)

        Assert.AreEqual("SomeClass", renderedOutput)


    [<Test>]
    member x.RendererUsedViaMonodoc_namespace() =
        let generator = getGenerator()

        let typeTemplate = "@inherits RazorTemplateBase
        @Model.Context[\"namespace\"]"

        generator.Add Templates.Namespace typeTemplate

        let renderedOutput = tree.RenderUrl("N:My.Sample", generator)

        Assert.AreEqual("My.Sample", renderedOutput)

    [<Test>]
    member x.RendererUsedViaMonodoc_method() =
        let generator = getGenerator() 

        let typeTemplate = "@inherits RazorTemplateBase
        @Raw(Model.Doc.Attribute(\"MemberName\").Value)"

        generator.Add Templates.Member typeTemplate

        let renderedOutput = tree.RenderUrl("M:My.Sample.SomeClass.GetName(string)", generator)

        Assert.AreEqual("GetName", renderedOutput)

    [<Test>]
    member x.FileNamespace() =
        let generator = getGenerator()

        generator |> loadNamespace |> loadShared

        let rendered = tree.RenderUrl("N:My.Sample", generator); 

        Assert.AreEqual(true, rendered.Contains("<h1>My.Sample"));


    [<Test>]
    member x.FileNamespaceTypeList() =
        let generator = getGenerator()

        generator |> loadNamespace |> loadShared

        let rendered = tree.RenderUrl("N:My.Sample", generator); 

        Assert.AreEqual(true, rendered.Contains("SomeClass"));

    [<Test>]
    member x.FileTypeMemberList() =
        let generator = getGenerator()

        generator |> loadType |> loadShared

        let rendered = tree.RenderUrl("T:My.Sample.SomeClass", generator); 

        Assert.AreEqual(true, rendered.Contains("GetName"));

    [<Test>]
    member x.FileNamespaceSummary() =
        let generator = getGenerator()

        generator |> loadNamespace |> loadShared

        let rendered = tree.RenderUrl("N:My.Sample", generator); 

        Assert.AreEqual(true, rendered.Contains("My.Sample Summary Text"));

    [<Test>]
    member x.FileTypeSummary() =
        let generator = getGenerator()

        generator |> loadType |> loadShared

        let rendered = tree.RenderUrl("T:My.Sample.SomeClass", generator); 

        Assert.AreEqual(true, rendered.Contains("My.Sample.SomeClass Summary Text"));

    [<Test>]
    member x.FileMemberRemarks() =
        let generator = getGenerator()

        generator |> loadMember |> loadShared

        let rendered = tree.RenderUrl("M:My.Sample.SomeClass.GetName(string)", generator); 

        Assert.AreEqual(true, rendered.Contains("My.Sample.SomeClass.GetName(string) Remarks"));

    [<Test>]
    member x.FileMemberParameterList() =
        let generator = getGenerator()

        generator |> loadMember |> loadShared

        let rendered = tree.RenderUrl("M:My.Sample.SomeClass.GetName(string)", generator); 

        Assert.AreEqual(true, rendered.Contains("prefix"));