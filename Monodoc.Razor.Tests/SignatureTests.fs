namespace Monodoc.Razor.Tests
open System
open System.Collections.Generic
open System.Xml.Linq

open NUnit.Framework
open Monodoc
open Monodoc.Razor

[<TestFixture>]
type SignatureTests() = 
    let typeDoc = "<Type Name=\"SomeClass\" FullName=\"My.Sample.SomeClass\">
  <TypeSignature Language=\"C#\" Value=\"public class SomeClass\" />
  <TypeSignature Language=\"ILAsm\" Value=\".class public auto ansi beforefieldinit SomeClass extends System.Object\" />
  <AssemblyInfo>
    <AssemblyName>SampleCode</AssemblyName>
    <AssemblyVersion>0.0.0.0</AssemblyVersion>
  </AssemblyInfo>
  <Base>
    <BaseTypeName>System.Object</BaseTypeName>
  </Base>
  <Interfaces />
  <Docs>
    <summary>My.Sample.SomeClass Summary Text</summary>
    <remarks>To be added.</remarks>
  </Docs>
  <Members>
    <Member MemberName=\".ctor\">
      <MemberSignature Language=\"C#\" Value=\"public SomeClass ();\" />
      <MemberSignature Language=\"ILAsm\" Value=\".method public hidebysig specialname rtspecialname instance void .ctor() cil managed\" />
      <MemberType>Constructor</MemberType>
      <AssemblyInfo>
        <AssemblyVersion>0.0.0.0</AssemblyVersion>
      </AssemblyInfo>
      <Parameters />
      <Docs>
        <summary>To be added.</summary>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>
    <Member MemberName=\"GetName\">
      <MemberSignature Language=\"C#\" Value=\"public string GetName (string prefix);\" />
      <MemberSignature Language=\"ILAsm\" Value=\".method public hidebysig instance string GetName(string prefix) cil managed\" />
      <MemberType>Method</MemberType>
      <AssemblyInfo>
        <AssemblyVersion>0.0.0.0</AssemblyVersion>
      </AssemblyInfo>
      <ReturnValue>
        <ReturnType>System.String</ReturnType>
      </ReturnValue>
      <Parameters>
        <Parameter Name=\"prefix\" Type=\"System.String\" />
      </Parameters>
      <Docs>
        <param name=\"prefix\">To be added.</param>
        <summary>To be added.</summary>
        <returns>To be added.</returns>
        <remarks>My.Sample.SomeClass.GetName(string) Remarks</remarks>
      </Docs>
    </Member>
  </Members>
</Type>
"
    [<Test>]
    member x.Text() =
        let doc = XDocument.Parse(typeDoc)
        let context = new Dictionary<string, string>()
        context.["show"] <- "typeoverview"
        let model = new EcmaModel(doc, context, "/")

        let actual = model.RenderSignature("C#")

        Assert.AreEqual ("public class SomeClass", actual)
