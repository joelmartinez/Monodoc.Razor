﻿namespace Monodoc.Razor.Tests
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