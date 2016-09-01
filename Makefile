
TestData/SampleCode.dll:
	mcs TestData/SampleCode.cs -target:library /doc:TestData/SampleCode.xml

SampleCodeDocs: TestData/SampleCode.dll
	mdoc update -i TestData/SampleCode.xml TestData/SampleCode.dll -o SampleCodeDocs
	cp TestData/ns-My.Sample.xml SampleCodeDocs/

SampleCode.tree: update
	cp TestData/ns-My.Sample.xml SampleCodeDocs/
	mdoc assemble -o SampleCode SampleCodeDocs
	mkdir -p Monodoc.Razor.Tests/bin/Debug/sources
	mkdir -p Monodoc.Razor.Tests/bin/Release/sources
	cp SampleCode.tree Monodoc.Razor.Tests/bin/Debug/sources
	cp SampleCode.tree Monodoc.Razor.Tests/bin/Release/sources
	cp SampleCode.zip Monodoc.Razor.Tests/bin/Debug/sources
	cp SampleCode.zip Monodoc.Razor.Tests/bin/Release/sources
	cp TestData/SampleCode.source Monodoc.Razor.Tests/bin/Debug/sources
	cp TestData/SampleCode.source Monodoc.Razor.Tests/bin/Release/sources
	cp TestData/monodoc.xml Monodoc.Razor.Tests/bin/Debug/
	cp TestData/monodoc.xml Monodoc.Razor.Tests/bin/Release/

build:TestData/SampleCode.dll
update: SampleCodeDocs
assemble: SampleCode.tree

clean: 
	rm TestData/SampleCode.dll TestData/SampleCode.xml SampleCode.tree SampleCode.zip
	rm -rf SampleCodeDocs
	rm -rf Monodoc.Razor.Tests/bin
	rm -rf Monodoc.Razor.Tests/obj