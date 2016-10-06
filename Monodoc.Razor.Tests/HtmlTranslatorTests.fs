namespace Monodoc.Razor.Tests
    open System
    open System.Collections.Generic

    open System.Xml.Linq

    open NUnit.Framework
    open Monodoc.Razor.HtmlTranslator

    [<TestFixture>]
    type HtmlTranslatorTests() = 

        [<Test>]
        member x.Text() =
            let doc = XDocument.Parse("<remarks>This is some text</remarks>")

            let actual = html doc.Root

            Assert.AreEqual ("This is some text", actual)



        [<Test>]
        member x.TextWithPara() =
            let doc = XDocument.Parse("<remarks>This <para>is</para> some text</remarks>")

            let actual = html doc.Root

            Assert.AreEqual ("This <p>is</p> some text", actual)


        [<Test>]
        member x.HideToBeAdded() =
            let doc = XDocument.Parse("<remarks>To be added.</remarks>")

            let actual = doc.Root |> html

            Assert.AreEqual ("", actual)

        [<Test>]
        member x.Block() =
            let doc = XDocument.Parse("<block>sometext</block>")

            let actual = doc.Root |> html

            Assert.AreEqual ("<div>sometext</div>", actual)


        [<Test>]
        member x.Block_Operator() =
            let doc = XDocument.Parse("<block type=\"behaviors\">sometext</block>")

            let actual = doc.Root |> html

            Assert.AreEqual ("<div><h3>Operation</h3>sometext</div>", actual)

        [<Test>]
        member x.Block_Note() =
            let doc = XDocument.Parse("<block type=\"note\">sometext</block>")

            let actual = doc.Root |> html

            Assert.AreEqual ("<div>Note: sometext</div>", actual)

        [<Test>]
        member x.Block_Overrides() =
            let doc = XDocument.Parse("<block type=\"overrides\">sometext</block>")

            let actual = doc.Root |> html

            Assert.AreEqual ("<div><h3>Note to Inheritors</h3>sometext</div>", actual)